using Microsoft.AspNetCore.Mvc;
using integracoes.Services.Consultas;
using System;
using System.Collections.Generic;
using System.Linq;


namespace integracoes.Controllers.Infos
{

    
    [ApiController]
    [Route("api/infos")]
    public class InfosController : ControllerBase
    {
        [HttpGet("status")]
        public IActionResult Status()
        {
            return Ok(new { status = "API de Consultas está operacional" });
        }

        [HttpGet("versao")]
        public IActionResult Versao()
        {
            var versao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Desconhecida";
            return Ok(new { versao });
        }

        [HttpGet("saudacao/{nome}")]
        public IActionResult Saudacao(string nome)
        {
            var mensagem = $"Olá, {nome}! Bem-vindo à API de Consultas.";
            return Ok(new { mensagem });
        }

        [HttpGet("tempo-resposta")]
        public IActionResult TempoResposta()
        {
            var tempoResposta = DateTime.UtcNow.ToString("o");
            return Ok(new { tempoResposta });
        }

        [HttpGet("info-servidor")]
        public IActionResult InfoServidor()
        {
            var nomeMaquina = Environment.MachineName;
            var sistemaOperacional = Environment.OSVersion.ToString();
            return Ok(new { nomeMaquina, sistemaOperacional });
        }

        [HttpGet("hora-atual")]
        public IActionResult HoraAtual()
        {
            var horaAtual = DateTime.UtcNow.ToString("o");
            return Ok(new { horaAtual });
        }

        [HttpGet("saudacao-hora")]
        public IActionResult SaudacaoHora()
        {
            var hora = DateTime.UtcNow.Hour;
            string saudacao = hora < 12 ? "Bom dia" : hora < 18 ? "Boa tarde" : "Boa noite";
            return Ok(new { saudacao });
        }

        [HttpGet("data-atual")]
        public IActionResult DataAtual()
        {
            var dataAtual = DateTime.UtcNow.ToString("yyyy-MM-dd");
            return Ok(new { dataAtual });
        }

        [HttpGet("fuso-horario")]
        public IActionResult FusoHorario()
        {
            var fusoHorario = TimeZoneInfo.Local.StandardName;
            return Ok(new { fusoHorario });
        }

        [HttpGet("uptime")]
        public IActionResult Uptime()
        {
            var uptime = (DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()).ToString();
            return Ok(new { uptime });
        }

        [HttpGet("memoria-uso")]
        public IActionResult MemoriaUso()
        {
            var memoriaUso = GC.GetTotalMemory(false);
            return Ok(new { memoriaUso });
        }

        [HttpGet("cpu-uso")]
        public IActionResult CpuUso()
        {
            // Placeholder: Implementação real de uso de CPU pode variar
            var cpuUso = "Informação de uso de CPU não implementada.";
            return Ok(new { cpuUso });
        }

        [HttpGet("processos")]
        public IActionResult Processos()
        {
            var processos = System.Diagnostics.Process.GetProcesses().Select(p => new { p.Id, p.ProcessName });
            return Ok(processos);
        }

        [HttpGet("threads")]
        public IActionResult Threads()
        {
            var threadCount = System.Diagnostics.Process.GetCurrentProcess().Threads.Count;
            return Ok(new { threadCount });
        }

        [HttpGet("arquitetura")]
        public IActionResult Arquitetura()
        {
            var arquitetura = Environment.Is64BitOperatingSystem ? "x64" : "x86";
            return Ok(new { arquitetura });
        }

        [HttpGet("dotnet-versao")]
        public IActionResult DotnetVersao()
        {
            var dotnetVersao = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
            return Ok(new { dotnetVersao });
        }

        [HttpGet("cultura")]
        public IActionResult Cultura()
        {
            var cultura = System.Globalization.CultureInfo.CurrentCulture.Name;
            return Ok(new { cultura });
        }

        [HttpGet("idioma")]
        public IActionResult Idioma()
        {
            var idioma = System.Globalization.CultureInfo.CurrentUICulture.Name;
            return Ok(new { idioma });
        }

        [HttpGet("timezone-info")]
        public IActionResult TimezoneInfo()
        {
            var timezone = TimeZoneInfo.Local;
            return Ok(new { timezone.Id, timezone.DisplayName, timezone.BaseUtcOffset });
        }

        [HttpGet("dias-atividade")]
        public IActionResult DiasAtividade()
        {
            var diasAtividade = (DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()).Days;
            return Ok(new { diasAtividade });
        }

        [HttpGet("horas-atividade")]
        public IActionResult HorasAtividade()
        {
            var horasAtividade = (DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()).TotalHours;
            return Ok(new { horasAtividade });
        }

        [HttpGet("minutos-atividade")]
        public IActionResult MinutosAtividade()
        {
            var minutosAtividade = (DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()).TotalMinutes;
            return Ok(new { minutosAtividade });
        }

        [HttpGet("segundos-atividade")]
        public IActionResult SegundosAtividade()
        {
            var segundosAtividade = (DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()).TotalSeconds;
            return Ok(new { segundosAtividade });
        }

        [HttpGet("cultura-info")]
        public IActionResult CulturaInfo()
        {
            var cultura = System.Globalization.CultureInfo.CurrentCulture;
            return Ok(new { cultura.Name, cultura.DisplayName, cultura.EnglishName });
        }

        [HttpGet("idioma-info")]
        public IActionResult IdiomaInfo()
        {
            var idioma = System.Globalization.CultureInfo.CurrentUICulture;
            return Ok(new { idioma.Name, idioma.DisplayName, idioma.EnglishName });
        }

        [HttpGet("timezone-detalhes")]
        public IActionResult TimezoneDetalhes()
        {
            var timezone = TimeZoneInfo.Local;
            return Ok(new { timezone.Id, timezone.DisplayName, timezone.StandardName, timezone.DaylightName, timezone.BaseUtcOffset });
        }

        [HttpGet("processador-info")]
        public IActionResult ProcessadorInfo()
        {
            var processador = Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER") ?? "Desconhecido";
            return Ok(new { processador });
        }

        [HttpGet("nucleos-cpu")]
        public IActionResult NucleosCpu()
        {
            var nucleos = Environment.ProcessorCount;
            return Ok(new { nucleos });
        }

        [HttpGet("arquitetura-processo")]
        public IActionResult ArquiteturaProcesso()
        {
            var arquiteturaProcesso = Environment.Is64BitProcess ? "x64" : "x86";
            return Ok(new { arquiteturaProcesso });
        }

        [HttpGet("sistema-operacional-detalhes")]
        public IActionResult SistemaOperacionalDetalhes()
        {
            var sistemaOperacional = Environment.OSVersion;
            return Ok(new { sistemaOperacional.Platform, sistemaOperacional.Version, sistemaOperacional.ServicePack });
        }

        [HttpGet("usuario-maquina")]
        public IActionResult UsuarioMaquina()
        {
            var usuario = Environment.UserName;
            return Ok(new { usuario });
        }

        [HttpGet("dominio-maquina")]
        public IActionResult DominioMaquina()
        {
            var dominio = Environment.UserDomainName;
            return Ok(new { dominio });
        }

        [HttpGet("pasta-temporaria")]
        public IActionResult PastaTemporaria()
        {
            var pastaTemp = System.IO.Path.GetTempPath();
            return Ok(new { pastaTemp });
        }

        [HttpGet("diretorio-atual")]
        public IActionResult DiretorioAtual()
        {
            var diretorioAtual = System.IO.Directory.GetCurrentDirectory();
            return Ok(new { diretorioAtual });
        }

        [HttpGet("variaveis-ambiente")]
        public IActionResult VariaveisAmbiente()
        {
            var variaveis = Environment.GetEnvironmentVariables();
            var dict = new Dictionary<string, string>();
            foreach (var key in variaveis.Keys)
            {
                dict[key.ToString() ?? ""] = variaveis[key]?.ToString() ?? "";
            }
            return Ok(dict);
        }

        [HttpGet("dotnet-runtime")]
        public IActionResult DotnetRuntime()
        {
            var runtime = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
            return Ok(new { runtime });
        }

        [HttpGet("dotnet-arch")]
        public IActionResult DotnetArch()
        {
            var arch = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString();
            return Ok(new { arch });
        }

        [HttpGet("dotnet-os")]
        public IActionResult DotnetOs()
        {
            var os = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            return Ok(new { os });
        }

        [HttpGet("dotnet-processador")]
        public IActionResult DotnetProcessador()
        {
            var processador = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString();
            return Ok(new { processador });
        }

        [HttpGet("dotnet-ambiente")]
        public IActionResult DotnetAmbiente()
        {
            var ambiente = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows) ? "Windows" :
                           System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux) ? "Linux" :
                           System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX) ? "macOS" : "Desconhecido";
            return Ok(new { ambiente });
        }
        [HttpGet("dotnet-versao-detalhes")]
        public IActionResult DotnetVersaoDetalhes()
        {
            var versaoDetalhes = new
            {
                FrameworkDescription = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
                FrameworkVersion = Environment.Version.ToString(),
                OSDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
                OSArchitecture = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString(),
                ProcessArchitecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString()
            };
            return Ok(versaoDetalhes);
        }

        [HttpGet("dotnet-detalhes-ambiente")]
        public IActionResult DotnetDetalhesAmbiente()
        {
            var detalhesAmbiente = new
            {
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                ProcessorCount = Environment.ProcessorCount,
                SystemDirectory = Environment.SystemDirectory,
                CurrentDirectory = Environment.CurrentDirectory,
                DotnetVersion = Environment.Version.ToString()
            };
            return Ok(detalhesAmbiente);
        }

        [HttpGet("dotnet-informacoes-sistema")]
        public IActionResult DotnetInformacoesSistema()
        {
            var informacoesSistema = new
            {
                OSVersion = Environment.OSVersion.ToString(),
                Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
                Is64BitProcess = Environment.Is64BitProcess,
                SystemPageSize = Environment.SystemPageSize,
                TickCount = Environment.TickCount,
                Uptime = (DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()).ToString()
            };
            return Ok(informacoesSistema);
        }

        [HttpGet("dotnet-informacoes-memoria")]
        public IActionResult DotnetInformacoesMemoria()
        {
            var informacoesMemoria = new
            {
                TotalMemory = GC.GetTotalMemory(false),
                GcCollectionCount0 = GC.CollectionCount(0),
                GcCollectionCount1 = GC.CollectionCount(1),
                GcCollectionCount2 = GC.CollectionCount(2)
            };
            return Ok(informacoesMemoria);
        }

        [HttpGet("dotnet-informacoes-processo")]
        public IActionResult DotnetInformacoesProcesso()
        {
            var processo = System.Diagnostics.Process.GetCurrentProcess();
            var informacoesProcesso = new
            {
                processo.Id,
                processo.ProcessName,
                StartTime = processo.StartTime.ToUniversalTime().ToString("o"),
                TotalProcessorTime = processo.TotalProcessorTime.ToString(),
                Threads = processo.Threads.Count,
                WorkingSet64 = processo.WorkingSet64
            };
            return Ok(informacoesProcesso);
        }

        [HttpGet("dotnet-informacoes-cpu")]
        public IActionResult DotnetInformacoesCpu()
        {
            // Placeholder: Implementação real de uso de CPU pode variar
            var informacoesCpu = new
            {
                CpuUsage = "Informação de uso de CPU não implementada."
            };
            return Ok(informacoesCpu);
        }

        [HttpGet("dotnet-informacoes-ambiente-completas")]
        public IActionResult DotnetInformacoesAmbienteCompletas()
        {
            var ambienteCompletas = new
            {
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                DomainName = Environment.UserDomainName,
                ProcessorCount = Environment.ProcessorCount,
                Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
                Is64BitProcess = Environment.Is64BitProcess,
                SystemDirectory = Environment.SystemDirectory,
                CurrentDirectory = Environment.CurrentDirectory,
                DotnetVersion = Environment.Version.ToString(),
                OSVersion = Environment.OSVersion.ToString(),
                TotalMemory = GC.GetTotalMemory(false)
            };
            return Ok(ambienteCompletas);
        }

        [HttpGet("dotnet-informacoes-completas")]
        public IActionResult DotnetInformacoesCompletas()
        {
            var informacoesCompletas = new
            {
                FrameworkDescription = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
                FrameworkVersion = Environment.Version.ToString(),
                OSDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
                OSArchitecture = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString(),
                ProcessArchitecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString(),
                IsWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows),
                IsLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux),
                IsMacOS = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX),
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                DomainName = Environment.UserDomainName,
                ProcessorCount = Environment.ProcessorCount,
                Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
                Is64BitProcess = Environment.Is64BitProcess,
                SystemDirectory = Environment.SystemDirectory,
                CurrentDirectory = Environment.CurrentDirectory,
                TotalMemory = GC.GetTotalMemory(false)
            };
            return Ok(informacoesCompletas);
        }

        [HttpGet("dotnet-saude")]
        public IActionResult DotnetSaude()
        {
            var saude = new
            {
                Status = "OK",
                Uptime = (DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()).ToString(),
                TotalMemory = GC.GetTotalMemory(false)
            };
            return Ok(saude);
        }

        [HttpGet("dotnet-metricas")]
        public IActionResult DotnetMetricas()
        {
            var metricas = new
            {
                Uptime = (DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()).ToString(),
                TotalMemory = GC.GetTotalMemory(false),
                GcCollectionCount0 = GC.CollectionCount(0),
                GcCollectionCount1 = GC.CollectionCount(1),
                GcCollectionCount2 = GC.CollectionCount(2)
            };
            return Ok(metricas);
        }

        [HttpGet("dotnet-resumo")]
        public IActionResult DotnetResumo()
        {
            var resumo = new
            {
                FrameworkDescription = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
                OSDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                ProcessorCount = Environment.ProcessorCount,
                Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
                TotalMemory = GC.GetTotalMemory(false)
            };
            return Ok(resumo);
        }

        [HttpGet("dotnet-detalhes-completos")]
        public IActionResult DotnetDetalhesCompletos()
        {
            var detalhesCompletos = new
            {
                FrameworkDescription = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
                FrameworkVersion = Environment.Version.ToString(),
                OSDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
                OSArchitecture = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString(),
                ProcessArchitecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString(),
                IsWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows),
                IsLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux),
                IsMacOS = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX),
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                DomainName = Environment.UserDomainName,
                ProcessorCount = Environment.ProcessorCount,
                Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
                Is64BitProcess = Environment.Is64BitProcess,
                SystemDirectory = Environment.SystemDirectory,
                CurrentDirectory = Environment.CurrentDirectory,
                TotalMemory = GC.GetTotalMemory(false),
                Uptime = (DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()).ToString()
            };
            return Ok(detalhesCompletos);
        }

        [HttpGet("dotnet-informacoes-avancadas")]
        public IActionResult DotnetInformacoesAvancadas()
        {
            var informacoesAvancadas = new
            {
                FrameworkDescription = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
                FrameworkVersion = Environment.Version.ToString(),
                OSDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
                OSArchitecture = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString(),
                ProcessArchitecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString(),
                IsWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows),
                IsLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux),
                IsMacOS = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX),
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                DomainName = Environment.UserDomainName,
                ProcessorCount = Environment.ProcessorCount,
                Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
                Is64BitProcess = Environment.Is64BitProcess,
                SystemDirectory = Environment.SystemDirectory,
                CurrentDirectory = Environment.CurrentDirectory,
                TotalMemory = GC.GetTotalMemory(false),
                Uptime = (DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()).ToString(),
                GcCollectionCount0 = GC.CollectionCount(0),
                GcCollectionCount1 = GC.CollectionCount(1),
                GcCollectionCount2 = GC.CollectionCount(2)
            };
            return Ok(informacoesAvancadas);
        }

        [HttpGet("dotnet-estado-saude")]
        public IActionResult DotnetEstadoSaude()
        {
            var estadoSaude = new
            {
                Status = "OK",
                Uptime = (DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()).ToString(),
                TotalMemory = GC.GetTotalMemory(false),
                GcCollectionCount0 = GC.CollectionCount(0),
                GcCollectionCount1 = GC.CollectionCount(1),
                GcCollectionCount2 = GC.CollectionCount(2)
            };
            return Ok(estadoSaude);
        }

        [HttpGet("dotnet-metricas-avancadas")]
        public IActionResult DotnetMetricasAvancadas()
        {
            var metricasAvancadas = new
            {
                Uptime = (DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()).ToString(),
                TotalMemory = GC.GetTotalMemory(false),
                GcCollectionCount0 = GC.CollectionCount(0),
                GcCollectionCount1 = GC.CollectionCount(1),
                GcCollectionCount2 = GC.CollectionCount(2)
            };
            return Ok(metricasAvancadas);
        }

        [HttpGet("dotnet-resumo-avancado")]
        public IActionResult DotnetResumoAvancado()
        {
            var resumoAvancado = new
            {
                FrameworkDescription = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
                OSDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                ProcessorCount = Environment.ProcessorCount,
                Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
                TotalMemory = GC.GetTotalMemory(false)
            };
            return Ok(resumoAvancado);
        }

        [HttpGet("dotnet-detalhes-avancados")]
        public
            IActionResult DotnetDetalhesAvancados()
        {
            var detalhesAvancados = new
            {
                FrameworkDescription = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
                FrameworkVersion = Environment.Version.ToString(),
                OSDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
                OSArchitecture = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString(),
                ProcessArchitecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString(),
                IsWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows),
                IsLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux),
                IsMacOS = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX),
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                DomainName = Environment.UserDomainName,
                ProcessorCount = Environment.ProcessorCount,
                Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
                Is64BitProcess = Environment.Is64BitProcess,
                SystemDirectory = Environment.SystemDirectory,
                CurrentDirectory = Environment.CurrentDirectory,
                TotalMemory = GC.GetTotalMemory(false),
                Uptime = (DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()).ToString()
            };
            return Ok(detalhesAvancados);
        }

        [HttpGet("dotnet-informacoes-completas-avancadas")]
        public IActionResult DotnetInformacoesCompletasAvancadas()
        {
            var informacoesCompletasAvancadas = new
            {
                FrameworkDescription = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
                FrameworkVersion = Environment.Version.ToString(),
                OSDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
                OSArchitecture = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString(),
                ProcessArchitecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString(),
                IsWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows),
                IsLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux),
                IsMacOS = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX),
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                DomainName = Environment.UserDomainName,
                ProcessorCount = Environment.ProcessorCount,
                Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
                Is64BitProcess = Environment.Is64BitProcess,
                SystemDirectory = Environment.SystemDirectory,
                CurrentDirectory = Environment.CurrentDirectory,
                TotalMemory = GC.GetTotalMemory(false),
                Uptime = (DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()).ToString()
            };
            return Ok(informacoesCompletasAvancadas);
        }

        [HttpGet("dotnet-saude-avancada")]
        public IActionResult DotnetSaudeAvancada()
        {
            var saudeAvancada = new
            {
                Status = "OK",
                Uptime = (DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()).ToString(),
                TotalMemory = GC.GetTotalMemory(false),
                GcCollectionCount0 = GC.CollectionCount(0),
                GcCollectionCount1 = GC.CollectionCount(1),
                GcCollectionCount2 = GC.CollectionCount(2)
            };
            return Ok(saudeAvancada);
        }

        [HttpGet("dotnet-metricas-completas-avancadas")]
        public IActionResult DotnetMetricasCompletasAvancadas()
        {
            var metricasCompletasAvancadas = new
            {
                Uptime = (DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()).ToString(),
                TotalMemory = GC.GetTotalMemory(false),
                GcCollectionCount0 = GC.CollectionCount(0),
                GcCollectionCount1 = GC.CollectionCount(1),
                GcCollectionCount2 = GC.CollectionCount(2)
            };
            return Ok(metricasCompletasAvancadas);
        }

        [HttpGet("dotnet-resumo-completo-avancado")]
        public IActionResult DotnetResumoCompletoAvancado()
        {
            var resumoCompletoAvancado = new
            {
                FrameworkDescription = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
                OSDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                ProcessorCount = Environment.ProcessorCount,
                Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
                TotalMemory = GC.GetTotalMemory(false)
            };
            return Ok(resumoCompletoAvancado);
        }

        [HttpGet("dotnet-detalhes-completos-avancados")]
        public IActionResult DotnetDetalhesCompletosAvancados()
        {
            var detalhesCompletosAvancados = new
            {
                FrameworkDescription = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
                FrameworkVersion = Environment.Version.ToString(),
                OSDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
                OSArchitecture = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString(),
                ProcessArchitecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString(),
                IsWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows),
                IsLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux),
                IsMacOS = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX),
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                DomainName = Environment.UserDomainName,
                ProcessorCount = Environment.ProcessorCount,
                Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
                Is64BitProcess = Environment.Is64BitProcess,
                SystemDirectory = Environment.SystemDirectory,
                CurrentDirectory = Environment.CurrentDirectory,
                TotalMemory = GC.GetTotalMemory(false),
                Uptime = (DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()).ToString()
            };
            return Ok(detalhesCompletosAvancados);
        }

        [HttpGet("dotnet-informacoes-ultimas")]
        public IActionResult DotnetInformacoesUltimas()
        {
            var informacoesUltimas = new
            {
                FrameworkDescription = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
                FrameworkVersion = Environment.Version.ToString(),
                OSDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
                OSArchitecture = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString(),
                ProcessArchitecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString(),
                IsWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows),
                IsLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux),
                IsMacOS = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX),
                MachineName = Environment.MachineName,
                UserName = Environment.UserName,
                DomainName = Environment.UserDomainName,
                ProcessorCount = Environment.ProcessorCount,
                Is64BitOperatingSystem = Environment.Is64BitOperatingSystem,
                Is64BitProcess = Environment.Is64BitProcess,
                SystemDirectory = Environment.SystemDirectory,
                CurrentDirectory = Environment.CurrentDirectory,
                TotalMemory = GC.GetTotalMemory(false),
                Uptime = (DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()).ToString()
            };
            return Ok(informacoesUltimas);

        }

        [HttpGet("dotnet-saude-completa-ultimas")]
        public IActionResult DotnetSaudeCompletaUltimas()
        {
            var saudeCompletaUltimas = new
            {
                Status = "OK",
                Uptime = (DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()).ToString(),
                TotalMemory = GC.GetTotalMemory(false),
                GcCollectionCount0 = GC.CollectionCount(0),
                GcCollectionCount1 = GC.CollectionCount(1),
                GcCollectionCount2 = GC.CollectionCount(2)
            };
            return Ok(saudeCompletaUltimas);
        }

        [HttpGet("dotnet-metricas-completas-ultimas")]
        public IActionResult DotnetMetricasCompletasUltimas()
        {
            var metricasCompletasUltimas = new
            {
                Uptime = (DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()).ToString(),
                TotalMemory = GC.GetTotalMemory(false),
                GcCollectionCount0 = GC.CollectionCount(0),
                GcCollectionCount1 = GC.CollectionCount(1),
                GcCollectionCount2 = GC.CollectionCount(2)
            };
            return Ok(metricasCompletasUltimas);

        }

    }
}




