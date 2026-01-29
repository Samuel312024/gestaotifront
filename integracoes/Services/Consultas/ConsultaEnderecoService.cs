using global::integracoes.Data;
using integracoes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace integracoes.Services.Consultas
{
    public class ConsultaEnderecoService : IConsultaEnderecoService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppDbContext _dbContext;
        private readonly ILogger<ConsultaEnderecoService> _logger;

        public ConsultaEnderecoService(
            IHttpClientFactory httpClientFactory,
            AppDbContext dbContext,
            ILogger<ConsultaEnderecoService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ConsultaEndereco?> BuscarPorCepAsync(string cep)
        {
            cep = cep.Trim().Replace("-", "").Replace(".", "");

            if (string.IsNullOrWhiteSpace(cep) || cep.Length != 8)
                return null;

            // Tenta buscar do banco primeiro (cache simples)
            ConsultaEndereco? consultaExistente = null;

            try
            {
                consultaExistente = await _dbContext.ConsultasEnderecos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Cep == cep);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar CEP {Cep} no banco", cep);
            }


            if (consultaExistente != null)
                return consultaExistente;

            // Busca na API ViaCEP
            var client = _httpClientFactory.CreateClient();
            var retryPolicy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            var response = await retryPolicy.ExecuteAsync(async () =>
                await client.GetAsync($"https://viacep.com.br/ws/{cep}/json/"));

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Falha ao consultar ViaCEP para CEP {Cep}", cep);
                return null;
            }

            var enderecoDto = await response.Content.ReadFromJsonAsync<ViaCepResponse>();

            if (enderecoDto == null || enderecoDto.Erro)
                return null;

            var novaConsulta = new ConsultaEndereco
            {
                Cep = cep,
                Logradouro = enderecoDto.Logradouro,
                Complemento = enderecoDto.Complemento,
                Bairro = enderecoDto.Bairro,
                Localidade = enderecoDto.Localidade,
                Uf = enderecoDto.Uf,
                Ibge = enderecoDto.Ibge,
                Gia = enderecoDto.Gia,
                Ddd = enderecoDto.Ddd,
                Siafi = enderecoDto.Siafi
            };

            _dbContext.ConsultasEnderecos.Add(novaConsulta);
            await _dbContext.SaveChangesAsync();

            return novaConsulta;
        }
    }

    // DTO auxiliar para ViaCEP
    internal class ViaCepResponse
    {
        public string Cep { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Localidade { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        public string Ibge { get; set; } = string.Empty;
        public string Gia { get; set; } = string.Empty;
        public string Ddd { get; set; } = string.Empty;
        public string Siafi { get; set; } = string.Empty;
        public bool Erro { get; set; }
    }
}