using Azure.Core;
using integracoes.Data;
using integracoes.Models;
using integracoes.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace integracoes.Controllers
{
    [ApiController]
    [Route("api/armazenar")]
    public class GuardarController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GuardarController(AppDbContext context)
        {
            _context = context;
        }

 
            [HttpPost("guardar")]
            public async Task<IActionResult> GuardarDados([FromBody] GuardarDadosRequest request)
            {
                if (request == null)
                    return BadRequest("Requisição inválida.");

                // 🔢 Remove tudo que não for número
                var cpf = CpfHelper.SomenteNumeros(request.Cpf);

                // ✅ Valida CPF real
                if (!CpfHelper.CpfValido(cpf))
                    return BadRequest("CPF inválido.");

                // 🔁 Verifica duplicidade no banco
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
            _context.RawDatas.RemoveRange(_context.RawDatas);
            await _context.SaveChangesAsync();
            return Ok(new { Mensagem = "Todos os dados foram removidos com sucesso" });
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarDados()
        {
            var dados = await _context.RawDatas.ToListAsync();
            return Ok(dados);
        }

        [HttpGet("obter/{id}")]
        public async Task<IActionResult> ObterDado(int id)
        {
            var dado = await _context.RawDatas.FindAsync(id);
            if (dado == null)
            {
                return NotFound(new { Mensagem = "Dado não encontrado" });
            }
            return Ok(dado);
        }

        [HttpDelete("remover/{id}")]
        public async Task<IActionResult> RemoverDado(int id)
        {
            var dado = await _context.RawDatas.FindAsync(id);
            if (dado == null)
            {
                return NotFound(new { Mensagem = "Dado não encontrado" });
            }
            _context.RawDatas.Remove(dado);
            await _context.SaveChangesAsync();
            return Ok(new { Mensagem = "Dado removido com sucesso" });
        }

        [HttpPut("atualizar/{id}")]
        public async Task<IActionResult> AtualizarDado(int id, [FromBody] object novosDados)
        {
            var dado = await _context.RawDatas.FindAsync(id);
            if (dado == null)
            {
                return NotFound(new { Mensagem = "Dado não encontrado" });
            }
            dado.Payload = JsonSerializer.Serialize(novosDados);
            await _context.SaveChangesAsync();
            return Ok(new { Mensagem = "Dado atualizado com sucesso" });
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarDados([FromQuery] string termo)
        {
            var dadosEncontrados = await _context.RawDatas
                .Where(d => d.Payload.Contains(termo))
                .ToListAsync();
            return Ok(dadosEncontrados);
        }
        
    }
}
