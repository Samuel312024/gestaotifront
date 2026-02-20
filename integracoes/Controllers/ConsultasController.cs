using integracoes.Services.Consultas;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/consultas")]
public class ConsultasController : ControllerBase
{
    private readonly IConsultaEnderecoService _enderecoService;
    private readonly IConsultaPlacaService _placaService;

    public ConsultasController(IConsultaEnderecoService enderecoService, IConsultaPlacaService placaService)
    {
        _enderecoService = enderecoService;
        _placaService = placaService;
    }

    [HttpGet("endereco/{cep}")]
    public async Task<IActionResult> BuscarEndereco(string cep)
    {
        var resultado = await _enderecoService.BuscarPorCepAsync(cep);

        if (resultado == null)
            return NotFound("CEP não encontrado ou inválido");

        return Ok(resultado);
    }

        

}