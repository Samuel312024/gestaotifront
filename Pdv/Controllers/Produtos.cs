using Microsoft.AspNetCore.Mvc;
using Pdv.DTOs;
using Pdv.Services;

namespace Pdv.Controllers;
[ApiController]
[Route("api/produtos")]
public class ProdutosController : ControllerBase
{
    private readonly ProdutoService _service;
    private readonly ProdutoService _produtoService;

    public ProdutosController(ProdutoService service)
    {
        _service = service;
        _produtoService = service;
    }

    [HttpPost]
    public async Task<IActionResult> Criar(CriarProdutoDto dto)
        => Ok(await _service.CriarAsync(dto));

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, AtualizarProdutoDto dto)
    {
        try
        {
            await _produtoService.AtualizarAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }


    [HttpGet("codigo/{codigo}")]
    public async Task<IActionResult> BuscarPorCodigo(string codigo)
        => Ok(await _service.BuscarPorCodigoAsync(codigo));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Inativar(int id)
    {
        await _service.InativarAsync(id);
        return NoContent();
    }
}
