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

    [HttpPatch("{id}/ativar")]
    public async Task<IActionResult> Ativar(int id)
    {
        try
        {
            await _produtoService.AtivarAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPatch("{id}/desativar")]
    public async Task<IActionResult> Desativar(int id)
    {
        try
        {
            await _produtoService.DesativarAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("scanner/{codigo}")]
    public async Task<IActionResult> Scanner(string codigo)
    {
        var produto = await _service.BuscarPorCodigoAsync(codigo);

        if (produto == null)
            return NotFound(new { precisaCadastro = true });

        return Ok(produto);
    }

    [HttpPost("scanner")]
    public async Task<IActionResult> Scanner(ScannerProdutoDto dto)
    {
        var produto = await _service.BuscarPorCodigoAsync(dto.Codigo);

        if (produto != null)
            return Ok(produto);

        // Auto-cadastro
        var novoProduto = await _service.CriarAsync(new CriarProdutoDto
        {
            Nome = dto.Nome ?? "Produto não identificado",
            Preco = dto.Preco ?? 0,
            Estoque = dto.Estoque ?? 0,
            Codigo = dto.Codigo
        });

        return Ok(novoProduto);
    }


}
