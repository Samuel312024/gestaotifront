using integracoes.Models;
using System.Threading.Tasks;

namespace integracoes.Services.Consultas
{
    public interface IConsultaEnderecoService
    {
        Task<ConsultasEnderecos?> BuscarPorCepAsync(string cep);
    }
}