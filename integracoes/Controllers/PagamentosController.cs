using Microsoft.AspNetCore.Mvc;
using integracoes.Services.Pagamentos;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class PagamentosController : ControllerBase
{
    private readonly IPagamentoService _pagamentoService;

    public PagamentosController(IPagamentoService pagamentoService)
    {
        _pagamentoService = pagamentoService;
    }

    [HttpPost("pix")]
    public async Task<IActionResult> CriarPix([FromBody] CreatePixRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _pagamentoService.CriarPagamentoPixAsync(dto);
        return Ok(result);
    }
}