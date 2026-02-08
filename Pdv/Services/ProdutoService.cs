using Integracoes.Pdv.Data;
using Microsoft.EntityFrameworkCore;
using Pdv.DTOs;
using Pdv.Models;

namespace Pdv.Services;
public class ProdutoService
{
    private readonly PdvContext _context;

    public ProdutoService(PdvContext context)
    {
        _context = context;
    }

    public async Task<Produto> CriarAsync(CriarProdutoDto dto)
    {
        if (await _context.Produtos.AnyAsync(p => p.CodigoBarras == dto.Codigo))
            throw new Exception("Produto já cadastrado.");

        var produto = new Produto
        {
            Nome = dto.Nome,
            CodigoBarras = dto.Codigo,
            PrecoAtual = dto.Preco
        };

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        _context.HistoricoPrecos.Add(new HistoricoPreco
        {
            ProdutoId = produto.Id,
            PrecoAnterior = 0,
            PrecoNovo = dto.Preco
        });

        await _context.SaveChangesAsync();

        return produto;
    }

    public async Task AtualizarAsync(int id, AtualizarProdutoDto dto)
    {
        var produto = await _context.Produtos
            .FirstOrDefaultAsync(p => p.Id == id && p.Ativo);

        if (produto == null)
            throw new KeyNotFoundException("Produto não encontrado.");

        if (produto.PrecoAtual != dto.Preco)
        {
            _context.HistoricoPrecos.Add(new HistoricoPreco
            {
                ProdutoId = produto.Id,
                PrecoAnterior = produto.PrecoAtual,
                PrecoNovo = dto.Preco,
                DataAlteracao = DateTime.Now
            });

            produto.PrecoAtual = dto.Preco;
        }

        produto.Nome = dto.Nome;

        await _context.SaveChangesAsync();
    }


    public async Task<Produto> BuscarPorCodigoAsync(string codigo)
    {
        return await _context.Produtos
            .FirstOrDefaultAsync(p => p.CodigoBarras == codigo && p.Ativo);
    }

    public async Task InativarAsync(int id)
    {
        var produto = await _context.Produtos
    .FirstOrDefaultAsync(p => p.Id == id);
        if (produto == null || !produto.Ativo)
            throw new Exception("Produto não encontrado.");


        produto.Ativo = false;
        await _context.SaveChangesAsync();
    }
}

