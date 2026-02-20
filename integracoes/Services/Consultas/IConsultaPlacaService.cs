using integracoes.Models;
using System.Threading.Tasks;

namespace integracoes.Services.Consultas
{
    public interface IConsultaPlacaService
    {
        Task<ConsultaPlaca?> BuscarPorPlacaAsync(string placa);
    }
}
