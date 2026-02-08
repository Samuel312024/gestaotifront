using Integracoes.Pdv.DTOs;
using Integracoes.Pdv.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/vendas")]
public class VendasController : ControllerBase
{
    private readonly VendaService _vendaService;

    public VendasController(VendaService vendaService)
    {
        _vendaService = vendaService;
    }

    [HttpPost]
    public async Task<IActionResult> Post(CriarVendaDto dto)
    {
        try
        {
            var venda = await _vendaService.CriarVendaAsync(dto);
            return Ok(venda);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.InnerException?.Message ?? ex.Message);
        }
    }
}
