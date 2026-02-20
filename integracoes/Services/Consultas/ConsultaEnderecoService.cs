using integracoes.Data;
using integracoes.DTOs.Consultas;
using integracoes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Http;
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

        // 🔹 Método público (orquestrador)
        public async Task<ConsultasEnderecos?> BuscarPorCepAsync(string cep)
        {
            cep = cep.Trim().Replace("-", "").Replace(".", "");

            if (string.IsNullOrWhiteSpace(cep) || cep.Length != 8)
                return null;

            // 1️⃣ Banco (cache)
            var cache = await BuscarNoBancoAsync(cep);
            if (cache != null)
                return cache;

            // 2️⃣ API ViaCEP
            var externo = await BuscarViaCepAsync(cep);
            if (externo == null)
                return null;

            // 3️⃣ Persistência
            await SalvarNoBancoAsync(externo);
            return externo;
        }

        // 🔹 Banco — somente leitura
        private async Task<ConsultasEnderecos?> BuscarNoBancoAsync(string cep)
        {
            try
            {
                return await _dbContext.ConsultasEnderecos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Cep == cep);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar CEP {Cep} no banco", cep);
                return null;
            }
        }

        // 🔹 API externa (ViaCEP) — SEM Polly manual
        private async Task<ConsultasEnderecos?> BuscarViaCepAsync(string cep)
        {
            var client = _httpClientFactory.CreateClient("SinespClient");

            var response = await client.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
            if (!response.IsSuccessStatusCode)
                return null;

            var dto = await response.Content.ReadFromJsonAsync<ViaCepResponse>();
            if (dto == null || dto.Erro)
                return null;

            return new ConsultasEnderecos
            {
                Cep = cep,
                Logradouro = dto.Logradouro,
                Complemento = dto.Complemento,
                Bairro = dto.Bairro,
                Localidade = dto.Localidade,
                Uf = dto.Uf,
                Ibge = dto.Ibge,
                Gia = dto.Gia,
                Ddd = dto.Ddd,
                Siafi = dto.Siafi
            };
        }

        // 🔹 Banco — somente escrita
        private async Task SalvarNoBancoAsync(ConsultasEnderecos endereco)
        {
            try
            {
                _dbContext.ConsultasEnderecos.Add(endereco);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar CEP {Cep} no banco", endereco.Cep);
            }
        }
    }
}
