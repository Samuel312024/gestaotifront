using integracoes.Data;
using integracoes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace integracoes.Services.Consultas
{
    public class ConsultaPlacaService : IConsultaPlacaService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppDbContext _dbContext;
        private readonly ILogger<ConsultaPlacaService> _logger;

        public ConsultaPlacaService(
            IHttpClientFactory httpClientFactory,
            AppDbContext dbContext,
            ILogger<ConsultaPlacaService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ConsultaPlaca?> BuscarPorPlacaAsync(string placa)
        {
            placa = placa.Trim().Replace("-", "").Replace(" ", "").ToUpper();

            if (string.IsNullOrWhiteSpace(placa) || placa.Length != 7)
                return null;

            var consultaExistente = await _dbContext.ConsultasPlacas
                .FirstOrDefaultAsync(p => p.Placa == placa);

            if (consultaExistente != null && consultaExistente.DataConsulta > DateTime.UtcNow.AddDays(-7))
                return consultaExistente;

            // Usa o HttpClient nomeado com as políticas Polly configuradas em Program.cs
            var client = _httpClientFactory.CreateClient("SinespClient");
            client.DefaultRequestHeaders.Remove("User-Agent");
            client.DefaultRequestHeaders.Add("User-Agent", "Sinesp-Cidadao/5.0.0 (Android 11; Pixel 5 Build/RQ3A.210805.001.A1)");

            var payload = new { placa = placa };

            var response = await client.PostAsJsonAsync("https://sinesp.md.gov.br/sinesp-cidadao/mobile/consultar-placa/v5", payload);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Falha na consulta Sinesp para placa {Placa}: {Status}", placa, response.StatusCode);
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var sinespResponse = JsonSerializer.Deserialize<SinespResponse>(json);

            if (sinespResponse == null || sinespResponse.CodigoRetorno != "0")
            {
                _logger.LogWarning("Sinesp retornou erro para placa {Placa}: {Mensagem}", placa, sinespResponse?.MensagemRetorno);
                return null;
            }

            var novaConsulta = new ConsultaPlaca
            {
                Placa = placa,
                Modelo = sinespResponse.Modelo ?? string.Empty,
                Marca = sinespResponse.Marca ?? string.Empty,
                Cor = sinespResponse.Cor ?? string.Empty,
                Ano = sinespResponse.Ano ?? string.Empty,
                Chassi = sinespResponse.Chassi?.Substring(0, Math.Min(10, sinespResponse.Chassi.Length)) ?? string.Empty,
                Situacao = sinespResponse.Situacao ?? "Não informado",
                Cidade = sinespResponse.Municipio ?? string.Empty,
                Uf = sinespResponse.Uf ?? string.Empty,
                Origem = "Sinesp Cidadão",
                DataConsulta = DateTime.UtcNow
            };

            _dbContext.ConsultasPlacas.Add(novaConsulta);
            await _dbContext.SaveChangesAsync();

            return novaConsulta;
        }
    }

    internal class SinespResponse
    {
        public string CodigoRetorno { get; set; } = string.Empty;
        public string MensagemRetorno { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Cor { get; set; } = string.Empty;
        public string Ano { get; set; } = string.Empty;
        public string Chassi { get; set; } = string.Empty;
        public string Situacao { get; set; } = string.Empty;
        public string Municipio { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
    }
}