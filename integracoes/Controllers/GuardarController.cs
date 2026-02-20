using Azure.Core;
using integracoes.Data;
using integracoes.DTOs;
using integracoes.DTOs.integracoes.Models.Requests;
using integracoes.Models;
using integracoes.Tools;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace integracoes.Controllers
{
    [ApiController]
    [Route("api/armazenar")]
    public class GuardarController : ControllerBase
    {
        private readonly AppDbContext _context;
        private static readonly string[] SupportedFormats = new[] { "json", "csv", "xml" };
        private static readonly string[] SupportedFileExtensions = new[] { "pdf", "png", "jpg", "jpeg", "txt", "csv", "json", "xml" };
        private readonly string UploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        private readonly string StorageFolder = Path.Combine(Directory.GetCurrentDirectory(), "storage");

        public GuardarController(AppDbContext context)
        {
            _context = context;
            Directory.CreateDirectory(UploadsFolder);
            Directory.CreateDirectory(StorageFolder);
        }

        // DTO usado para retornar RawData sem materializar a entidade completa,
        // evitando exceções quando colunas no banco podem ser NULL embora o modelo
        // possua tipos não anuláveis.
        private sealed record RawDataDto
        {
            public int Id { get; init; }
            public string? Payload { get; init; }
            public string? Cpf { get; init; }
            public DateTime? ReceivedAt { get; init; }
        }

        // Retorna uma IQueryable projetada com EF.Property<...> para evitar materializar
        // propriedades como tipos não anuláveis no CLR quando o valor no DB for NULL.
        private IQueryable<RawDataDto> SafeRawDatasQuery()
        {
            return _context.RawDatas
                .AsNoTracking()
                .Select(d => new RawDataDto
                {
                    Id = EF.Property<int>(d, "Id"),
                    Payload = EF.Property<string?>(d, "Payload"),
                    Cpf = EF.Property<string?>(d, "Cpf"),
                    ReceivedAt = EF.Property<DateTime?>(d, "ReceivedAt")
                });
        }

        [HttpGet("formatos")]
        public IActionResult ObterFormatosSuportados()
        {
            var formatos = SupportedFormats.Select(f => new
            {
                Nome = f,
                Descricao = f switch
                {
                    "json" => "Formato JSON (suporta objetos e coleções).",
                    "csv" => "Formato CSV (suporta coleções homogêneas).",
                    "xml" => "Formato XML (suporta objeto único ou coleções conforme raiz).",
                    _ => string.Empty
                }
            });
            return Ok(formatos);
        }


        [HttpPost("upload")]
        [RequestSizeLimit(104857600)]
        public async Task<IActionResult> UploadFiles([FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return BadRequest("Nenhum arquivo enviado.");

            var resultado = new List<object>();

            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                    continue;

                var nomeOriginal = Path.GetFileName(file.FileName);
                var ext = Path.GetExtension(nomeOriginal)
                              .TrimStart('.')
                              .ToLowerInvariant();

                var nomeSalvo = $"{Guid.NewGuid():N}_{nomeOriginal}";
                var caminho = Path.Combine(UploadsFolder, nomeSalvo);

                if (!Directory.Exists(UploadsFolder))
                    Directory.CreateDirectory(UploadsFolder);

                using (var fs = new FileStream(caminho, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }

                var arquivo = new ArquivoUpload
                {
                    NomeOriginal = nomeOriginal,
                    NomeSalvo = nomeSalvo,
                    Extensao = ext,
                    TamanhoBytes = file.Length,
                    Caminho = caminho,
                    Status = "Uploaded",
                    Erro = string.Empty,
                    TabelaDestino = string.Empty,
                    DataUpload = DateTime.UtcNow,
                };

                _context.ArquivosUpload.Add(arquivo);
                await _context.SaveChangesAsync();

                resultado.Add(new
                {
                    arquivo.Id,
                    arquivo.NomeOriginal,
                    arquivo.NomeSalvo,
                    arquivo.Extensao,
                    arquivo.TabelaDestino,
                    arquivo.TamanhoBytes,
                    arquivo.DataUpload

                });
            }


            return Ok(new
            {
                Mensagem = "Upload realizado e registrado no banco",
                Arquivos = resultado
            });
        }

        [HttpPost("processar/{id}")]
        public async Task<IActionResult> Processar(int id)
        {
            var arquivo = await _context.ArquivosUpload.FindAsync(id);

            if (arquivo == null)
                return NotFound();

            try
            {
                // lógica de processamento aqui

                arquivo.Status = "Processed";
                arquivo.DataProcessamento = DateTime.UtcNow;
                arquivo.Erro = string.Empty;
            }
            catch (Exception ex)
            {
                arquivo.Status = "Error";
                arquivo.Erro = ex.Message ?? string.Empty;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }



        [HttpGet("uploaded")]
        public IActionResult ListUploadedFiles()
        {
            if (!Directory.Exists(UploadsFolder))
                return Ok(new List<object>());

            var files = Directory.GetFiles(UploadsFolder)
                .Select(f => new FileInfo(f))
                .Select(fi => new { Nome = fi.Name, Size = fi.Length, Ext = fi.Extension.TrimStart('.') })
                .ToList();

            return Ok(files);
        }

        [HttpPost("processar-arquivo")]
        public async Task<IActionResult> ProcessarArquivo([FromBody] ProcessarArquivoRequest req)
        {
            if (req == null)
                return BadRequest("Requisição inválida.");

            var arquivo = await _context.ArquivosUpload.FindAsync(req.ArquivoId);

            if (arquivo == null)
                return NotFound("Arquivo não encontrado.");

            if (!System.IO.File.Exists(arquivo.Caminho))
                return BadRequest("Arquivo físico não existe.");

            try
            {
                var content = await System.IO.File.ReadAllTextAsync(arquivo.Caminho);
                var ext = arquivo.Extensao;

                var itens = ext switch
                {
                    "json" => ParseContentToElements(content, "json"),
                    "csv" => ParseCsvToJsonElements(content),
                    "xml" => ParseXmlToJsonElements(content),
                    _ => new List<JsonElement>()
                };

                int adicionados = 0;

                if (string.Equals(req.TargetTabela, "rawdatas", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var item in itens)
                    {
                        var payload = JsonSerializer.Serialize(item);
                        var cpf = ExtractCpfFromJsonElement(item, payload);

                        if (!string.IsNullOrEmpty(cpf))
                        {
                            cpf = CpfHelper.SomenteNumeros(cpf);
                            if (!CpfHelper.CpfValido(cpf)) continue;
                            if (await _context.RawDatas.AnyAsync(x => x.Cpf == cpf)) continue;
                        }

                        _context.RawDatas.Add(new RawData
                        {
                            Payload = payload,
                            Cpf = cpf ?? "",
                            ReceivedAt = DateTime.UtcNow
                        });

                        adicionados++;
                    }

                    await _context.SaveChangesAsync();
                }

                arquivo.Status = "Processed";
                arquivo.TabelaDestino = req.TargetTabela ?? string.Empty;
                arquivo.DataProcessamento = DateTime.UtcNow;

                arquivo.Erro = string.Empty;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Mensagem = "Arquivo processado com sucesso",
                    Adicionados = adicionados
                });
            }
            catch (Exception ex)
            {
                arquivo.Status = "Error";
                arquivo.Erro = ex.Message ?? string.Empty;
                await _context.SaveChangesAsync();

                return StatusCode(500, "Erro ao processar arquivo.");
            }
        }

        private IQueryable? GetQueryableByName(string tabela)
        {
            var prop = _context.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(p =>
                    p.PropertyType.IsGenericType &&
                    p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                    p.Name.Equals(tabela, StringComparison.OrdinalIgnoreCase));

            if (prop == null)
                return null;

            return prop.GetValue(_context) as IQueryable;
        }


        [HttpGet("exportar-tabela")]
        public async Task<IActionResult> ExportarTabela([FromQuery] string tabela, [FromQuery] string formato = "json")
        {
            if (string.IsNullOrWhiteSpace(tabela))
                return BadRequest("Parâmetro 'tabela' é obrigatório.");

            formato = (formato ?? "json").ToLowerInvariant();
            if (!SupportedFormats.Contains(formato))
                return BadRequest($"Formato inválido. Use: {string.Join(", ", SupportedFormats)}");

            var queryable = GetQueryableByName(tabela);
            if (queryable == null)
                return BadRequest($"Tabela '{tabela}' não encontrada no DbContext.");

            var elementType = queryable.ElementType;

            // Obtém propriedades públicas
            var props = elementType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead)
                .ToArray();

            var propNames = props.Select(p => p.Name).ToArray();

            // Monta expressão: e => new object[] { EF.Property<object>(e, "Prop1"), ... }
            var param = System.Linq.Expressions.Expression.Parameter(elementType, "e");

            var efPropertyMethod = typeof(EF)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(m => m.Name == "Property" && m.IsGenericMethodDefinition && m.GetParameters().Length == 2);

            var propExpressions = props.Select(p =>
            {
                var propAccess = System.Linq.Expressions.Expression.Call(typeof(EF),"Property",new[] { p.PropertyType },param, Expression.Constant(p.Name));

                // converte para object para construir object[] corretamente
                return System.Linq.Expressions.Expression.Convert(propAccess, typeof(object));
            }).ToArray();

            var arrayExpression = System.Linq.Expressions.Expression.NewArrayInit(typeof(object), propExpressions);
            var lambda = System.Linq.Expressions.Expression.Lambda(arrayExpression, param);

            // Aplica Queryable.Select<TSource, TResult>
            var selectMethodDef = typeof(Queryable).GetMethods()
                .First(m => m.Name == "Select" && m.GetParameters().Length == 2);

            var selectMethod = selectMethodDef.MakeGenericMethod(elementType, typeof(object[]));
            var projectedQueryableObj = selectMethod.Invoke(null, new object[] { queryable, lambda });
            if (projectedQueryableObj is not IQueryable projectedQueryable)
                return StatusCode(500, "Erro ao projetar consulta.");

            // Executa ToListAsync<object[]>
            var toListAsyncMethod = typeof(EntityFrameworkQueryableExtensions).GetMethods()
                .First(m => m.Name == "ToListAsync" && m.IsGenericMethod && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(object[]));

            var taskObj = toListAsyncMethod.Invoke(null, new object[] { projectedQueryable, HttpContext.RequestAborted });
            if (taskObj == null)
                return StatusCode(500, "Erro ao executar consulta.");

            var task = (Task)taskObj;
            await task.ConfigureAwait(false);

            var result = task.GetType().GetProperty("Result")!.GetValue(task) as IList;
            var rows = new List<object[]>();
            if (result != null)
            {
                foreach (var item in result)
                {
                    if (item is object[] arr)
                        rows.Add(arr);
                }
            }

            // Converte para lista de dicionários (nome -> valor)
            var dicts = rows.Select(r =>
            {
                var dict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < propNames.Length; i++)
                {
                    object? val = i < r.Length ? r[i] : null;
                    dict[propNames[i]] = val;
                }
                return dict;
            }).ToList();
            // Serializa conforme formato

            if (formato == "json")
            {
                var json = JsonSerializer.Serialize(dicts, new JsonSerializerOptions { WriteIndented = true });
                return File(Encoding.UTF8.GetBytes(json), "application/json", $"{tabela}.json");
            }

            if (formato == "csv")
            {
                var csv = GenerateCsvFromData(propNames, dicts);
                var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv)).ToArray();

                return File(bytes, "text/csv", $"{tabela}.csv");

            }

            // xml
            var xmlBytes = SerializeDictionaryCollectionToXml(dicts, tabela);
            return File(xmlBytes, "application/xml", $"{tabela}.xml");
        }

        private string GenerateCsvFromData(string[] headers, List<Dictionary<string, object?>> rows)
        {
            var sb = new StringBuilder();

            // Cabeçalho igual ao SQL
            sb.AppendLine(string.Join(";", headers));

            foreach (var row in rows)
            {
                var values = headers.Select(h =>
                {
                    row.TryGetValue(h, out var v);

                    if (v == null)
                        return "NULL"; // 👈 igual SQL Server

                    if (v is DateTime dt)
                        return dt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                    return EscapeCsv(v.ToString() ?? "NULL");
                });

                sb.AppendLine(string.Join(";", values));
            }

            return sb.ToString();
        }


        private byte[] SerializeDictionaryCollectionToXml(List<Dictionary<string, object?>> rows, string rootName)
        {
            var doc = new XDocument();
            var root = new XElement(rootName);
            doc.Add(root);

            foreach (var row in rows)
            {
                var itemEl = new XElement("Item");
                foreach (var kv in row)
                {
                    var name = XmlConvert.EncodeName(kv.Key ?? "Field");
                    string valueStr = kv.Value switch
                    {
                        null => string.Empty,
                        DateTime dt => dt.ToString("o", CultureInfo.InvariantCulture),
                        _ => Convert.ToString(kv.Value, CultureInfo.InvariantCulture) ?? string.Empty
                    };
                    itemEl.Add(new XElement(name, valueStr));
                }
                root.Add(itemEl);
            }

            using var ms = new MemoryStream();
            doc.Save(ms);
            return ms.ToArray();
        }
    
        private string EscapeCsv(string s)
        {
            if (s == null) return "";
            if (s.Contains('"')) s = s.Replace("\"", "\"\"");
            if (s.Contains(',') || s.Contains('\n') || s.Contains('\r') || s.Contains('"'))
                return $"\"{s}\"";
            return s;
        }

        // Import helpers (reused/adaptados)

        private List<JsonElement> ParseCsvToJsonElements(string csv)
        {
            var lines = csv.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                           .Where(l => !string.IsNullOrWhiteSpace(l))
                           .ToArray();
            var result = new List<JsonElement>();
            if (lines.Length == 0)
                return result;

            var header = SplitCsvLine(lines[0]).ToArray();
            for (int i = 1; i < lines.Length; i++)
            {
                var fields = SplitCsvLine(lines[i]).ToArray();
                var obj = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                for (int c = 0; c < header.Length; c++)
                {
                    var key = header[c];
                    var val = c < fields.Length ? fields[c] : string.Empty;
                    obj[key] = val;
                }
                var json = JsonSerializer.Serialize(obj);
                var je = JsonSerializer.Deserialize<JsonElement>(json);
                result.Add(je);
            }
            return result;
        }

        private IEnumerable<string> SplitCsvLine(string line)
        {
            var fields = new List<string>();
            if (line == null) return fields;
            var sb = new StringBuilder();
            bool inQuotes = false;
            for (int i = 0; i < line.Length; i++)
            {
                var ch = line[i];
                if (ch == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        sb.Append('"');
                        i++; // skip escaped quote
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (ch == ',' && !inQuotes)
                {
                    fields.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(ch);
                }
            }
            fields.Add(sb.ToString());
            return fields.Select(f => TrimSurroundingQuotes(f)).ToList();
        }

        private string TrimSurroundingQuotes(string s)
        {
            if (s == null) return string.Empty;
            if (s.Length >= 2 && s[0] == '"' && s[^1] == '"')
                return s.Substring(1, s.Length - 2).Replace("\"\"", "\"");
            return s;
        }

        private List<JsonElement> ParseXmlToJsonElements(string xml)
        {
            var result = new List<JsonElement>();
            XDocument doc;
            try
            {
                doc = XDocument.Parse(xml);
            }
            catch
            {
                return result;
            }

            var root = doc.Root;
            if (root == null)
                return result;

            var groups = root.Elements().GroupBy(e => e.Name.LocalName).ToList();
            bool isCollection = groups.Any(g => g.Count() > 1);

            if (isCollection)
            {
                var repeated = groups.Where(g => g.Count() > 1).FirstOrDefault();
                if (repeated != null)
                {
                    foreach (var itemEl in repeated)
                    {
                        var dict = XElementToDictionary(itemEl);
                        var json = JsonSerializer.Serialize(dict);
                        var je = JsonSerializer.Deserialize<JsonElement>(json);
                        result.Add(je);
                    }
                    return result;
                }
            }

            var children = root.Elements().ToArray();
            bool childrenAreItems = children.Length > 0 && children.All(c => c.HasElements);

            if (childrenAreItems)
            {
                foreach (var child in children)
                {
                    var dict = XElementToDictionary(child);
                    var json = JsonSerializer.Serialize(dict);
                    var je = JsonSerializer.Deserialize<JsonElement>(json);
                    result.Add(je);
                }
            }
            else
            {
                var dict = XElementToDictionary(root);
                var json = JsonSerializer.Serialize(dict);
                var je = JsonSerializer.Deserialize<JsonElement>(json);
                result.Add(je);
            }

            return result;
        }

        private object XElementToDictionary(XElement el)
        {
            if (!el.HasElements)
                return el.Value;

            var dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            foreach (var g in el.Elements().GroupBy(e => e.Name.LocalName))
            {
                var name = g.Key;
                var items = g.ToList();
                if (items.Count > 1)
                {
                    var list = new List<object>();
                    foreach (var item in items)
                        list.Add(XElementToDictionary(item));
                    dict[name] = list;
                }
                else
                {
                    dict[name] = XElementToDictionary(items.First());
                }
            }
            return dict;
        }

        // util: extrai cpf de JsonElement (propriedades comuns ou regex)
        private string ExtractCpfFromJsonElement(JsonElement item, string payloadStr)
        {
            string cpfLimpo = string.Empty;
            if (item.ValueKind == JsonValueKind.Object)
            {
                if (item.TryGetProperty("Cpf", out var p) || item.TryGetProperty("cpf", out p) || item.TryGetProperty("CPF", out p))
                {
                    if (p.ValueKind == JsonValueKind.String)
                        cpfLimpo = CpfHelper.SomenteNumeros(p.GetString() ?? string.Empty);
                    else
                        cpfLimpo = CpfHelper.SomenteNumeros(p.GetRawText());
                }
            }
            if (string.IsNullOrEmpty(cpfLimpo))
            {
                var m = Regex.Match(payloadStr, @"\d{11}");
                if (m.Success)
                    cpfLimpo = m.Value;
            }
            return cpfLimpo;
        }

        private List<JsonElement> ParseContentToElements(string content, string formato)
        {
            formato = formato?.ToLowerInvariant() ?? "json";
            if (formato == "json")
            {
                JsonElement root;
                root = JsonSerializer.Deserialize<JsonElement>(content);
                var items = new List<JsonElement>();
                if (root.ValueKind == JsonValueKind.Array)
                {
                    foreach (var el in root.EnumerateArray()) items.Add(el);
                }
                else if (root.ValueKind == JsonValueKind.Object)
                {
                    items.Add(root);
                }
                return items;
            }
            else if (formato == "csv")
                return ParseCsvToJsonElements(content);
            else // xml
                return ParseXmlToJsonElements(content);
        }

        // ----- Métodos existentes do controller (ajustados para evitar SqlNullValueException) -----
        [HttpPost("guardar")]
        public async Task<IActionResult> GuardarDados([FromBody] GuardarDadosRequest request)
        {
            if (request == null)
                return BadRequest("Requisição inválida.");

            var cpf = CpfHelper.SomenteNumeros(request.Cpf);

            if (!CpfHelper.CpfValido(cpf))
                return BadRequest("CPF inválido.");

            bool cpfJaExiste = await _context.RawDatas
                .AnyAsync(x => x.Cpf == cpf);

            if (cpfJaExiste)
                return Conflict("CPF já cadastrado.");

            var entry = new RawData
            {
                Payload = System.Text.Json.JsonSerializer.Serialize(request.Nome),
                ReceivedAt = DateTime.UtcNow,
                Cpf = cpf
            };

            _context.RawDatas.Add(entry);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Mensagem = "Dados guardados com sucesso",
                entry.Id
            });
        }

        [HttpDelete("limpar")]
        public async Task<IActionResult> LimparDados()
        {
            // Evita materializar entidades: usa delete em massa via EF Core
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM RawDatas");
            return Ok(new { Mensagem = "Todos os dados foram removidos com sucesso" });
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarDados()
        {
            var dados = await SafeRawDatasQuery().ToListAsync();
            return Ok(dados);
        }

        [HttpGet("obter/{id}")]
        public async Task<IActionResult> ObterDado(int id)
        {
            var dado = await SafeRawDatasQuery().FirstOrDefaultAsync(d => d.Id == id);
            if (dado == null)
            {
                return NotFound(new { Mensagem = "Dado não encontrado" });
            }
            return Ok(dado);
        }

        [HttpDelete("remover/{id}")]
        public async Task<IActionResult> RemoverDado(int id)
        {
            var affected = await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM RawDatas WHERE Id = {id}");
            if (affected == 0)
                return NotFound(new { Mensagem = "Dado não encontrado" });

            return Ok(new { Mensagem = "Dado removido com sucesso" });
        }

        [HttpPut("atualizar/{id}")]
        public async Task<IActionResult> AtualizarDado(int id, [FromBody] object novosDados)
        {
            var json = JsonSerializer.Serialize(novosDados);
            var affected = await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE RawDatas SET Payload = {json} WHERE Id = {id}");
            if (affected == 0)
                return NotFound(new { Mensagem = "Dado não encontrado" });

            return Ok(new { Mensagem = "Dado atualizado com sucesso" });
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarDados([FromQuery] string termo)
        {
            if (string.IsNullOrEmpty(termo))
                return BadRequest("Termo de busca inválido.");

            // Faz busca no banco sem materializar entidades inteiras
            var encontrados = await SafeRawDatasQuery()
                .Where(d => (d.Payload ?? "").Contains(termo))
                .ToListAsync();

            return Ok(encontrados);
        }

        [HttpPatch("atualizar-cpf/{id}")]
        public async Task<IActionResult> AtualizarCpf(int id, [FromBody] string novoCpf)
        {
            var exists = await _context.RawDatas.AnyAsync(x => x.Id == id);
            if (!exists)
                return NotFound(new { Mensagem = "Dado não encontrado" });

            var cpfLimpo = CpfHelper.SomenteNumeros(novoCpf);
            if (!CpfHelper.CpfValido(cpfLimpo))
            {
                return BadRequest("CPF inválido.");
            }
            bool cpfJaExiste = await _context.RawDatas
                .AnyAsync(x => x.Cpf == cpfLimpo && x.Id != id);
            if (cpfJaExiste)
            {
                return Conflict("CPF já cadastrado em outro registro.");
            }

            var affected = await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE RawDatas SET Cpf = {cpfLimpo} WHERE Id = {id}");
            if (affected == 0)
                return NotFound(new { Mensagem = "Dado não encontrado" });

            return Ok(new { Mensagem = "CPF atualizado com sucesso" });
        }

        [HttpPost("exportar")]
        public async Task<IActionResult> ExportarDados()
        {
            var dados = await SafeRawDatasQuery().ToListAsync();
            var json = JsonSerializer.Serialize(dados, new JsonSerializerOptions { WriteIndented = true });
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            return File(bytes, "application/json", "dados_exportados.json");
        }

        [HttpPost("importar")]
        public async Task<IActionResult> ImportarDados([FromQuery] string formato = "json", [FromBody] string? content = null)
        {
            try
            {
                formato = (formato ?? "json").ToLowerInvariant();
                if (!SupportedFormats.Contains(formato))
                    return BadRequest($"Formato inválido. Use: {string.Join(", ", SupportedFormats)}");

                if (string.IsNullOrWhiteSpace(content))
                    return BadRequest("Corpo da requisição vazio.");

                List<JsonElement> itens;

                if (formato == "json")
                {
                    JsonElement root;
                    try
                    {
                        root = JsonSerializer.Deserialize<JsonElement>(content);
                    }
                    catch (JsonException)
                    {
                        return BadRequest("JSON inválido.");
                    }

                    itens = new List<JsonElement>();
                    if (root.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var el in root.EnumerateArray())
                            itens.Add(el);
                    }
                    else if (root.ValueKind == JsonValueKind.Object)
                    {
                        itens.Add(root);
                    }
                    else
                    {
                        return BadRequest("JSON deve ser um objeto ou um array de objetos.");
                    }
                }
                else if (formato == "csv")
                {
                    itens = ParseCsvToJsonElements(content);
                }
                else // xml
                {
                    itens = ParseXmlToJsonElements(content);
                }

                int adicionados = 0;
                int puladosInvalidosCpf = 0;
                int puladosDuplicadosCpf = 0;

                foreach (var item in itens)
                {
                    var payloadStr = JsonSerializer.Serialize(item, new JsonSerializerOptions { WriteIndented = false });

                    string cpfLimpo = string.Empty;
                    if (item.ValueKind == JsonValueKind.Object)
                    {
                        if (item.TryGetProperty("Cpf", out var p) || item.TryGetProperty("cpf", out p) || item.TryGetProperty("CPF", out p))
                        {
                            if (p.ValueKind == JsonValueKind.String)
                                cpfLimpo = CpfHelper.SomenteNumeros(p.GetString() ?? string.Empty);
                            else
                                cpfLimpo = CpfHelper.SomenteNumeros(p.GetRawText());
                        }
                    }

                    if (string.IsNullOrEmpty(cpfLimpo))
                    {
                        var m = Regex.Match(payloadStr, @"\d{11}");
                        if (m.Success)
                            cpfLimpo = m.Value;
                    }

                    bool temCpfDetectado = !string.IsNullOrEmpty(cpfLimpo);
                    bool cpfValido = false;

                    if (temCpfDetectado)
                    {
                        cpfLimpo = CpfHelper.SomenteNumeros(cpfLimpo);
                        cpfValido = CpfHelper.CpfValido(cpfLimpo);
                        if (!cpfValido)
                        {
                            puladosInvalidosCpf++;
                            continue;
                        }

                        bool existe = await _context.RawDatas.AnyAsync(x => x.Cpf == cpfLimpo);
                        if (existe)
                        {
                            puladosDuplicadosCpf++;
                            continue;
                        }
                    }
                    else
                    {
                        cpfLimpo = string.Empty;
                    }

                    var entry = new RawData
                    {
                        Payload = payloadStr,
                        ReceivedAt = DateTime.UtcNow,
                        Cpf = cpfLimpo
                    };

                    _context.RawDatas.Add(entry);
                    adicionados++;
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Mensagem = "Importação finalizada",
                    Adicionados = adicionados,
                    PuladosPorCpfInvalido = puladosInvalidosCpf,
                    PuladosPorCpfDuplicado = puladosDuplicadosCpf
                });
            }
            catch (JsonException)
            {
                return BadRequest("Formato JSON inválido.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensagem = "Erro interno durante a importação.", Detalhes = ex.Message });
            }
        }
    }
}
