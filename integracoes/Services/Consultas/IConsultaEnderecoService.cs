using integracoes.Models;
using System.Threading.Tasks;

namespace integracoes.Services.Consultas
{
    public interface IConsultaEnderecoService
    {
        Task<ConsultaEndereco?> BuscarPorCepAsync(string cep);
    }
}