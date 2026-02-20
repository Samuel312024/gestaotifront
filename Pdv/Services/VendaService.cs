using Integracoes.Pdv.Data;
using Integracoes.Pdv.DTOs;
using Integracoes.Pdv.Models;
using Microsoft.EntityFrameworkCore;
using Pdv.DTOs;
using Pdv.Models;

namespace Integracoes.Pdv.Services;

public class VendaService
{
    private readonly PdvContext _context;

    public VendaService(PdvContext context)
    {
        _context = context;
    }

    public async Task<Venda> CriarVendaAsync(CriarVendaDto dto)
    {
        if (dto.Itens == null || !dto.Itens.Any())
            throw new Exception("Venda precisa ter itens.");

        var venda = new Venda
        {
            DataVenda = DateTime.Now,
            FormaPagamento = dto.FormaPagamento,
            Itens = new List<VendaItem>()
        };

        foreach (var item in dto.Itens)
        {
            var produto = await _context.Produtos
                .FirstOrDefaultAsync(p => p.Id == item.ProdutoId);

            if (produto == null)
                throw new Exception($"Produto {item.ProdutoId} não encontrado.");

            var vendaItem = new VendaItem
            {
                ProdutoId = produto.Id,
                Quantidade = item.Quantidade,
                PrecoUnitario = produto.Preco,
                Subtotal = produto.Preco * item.Quantidade
            };

            venda.Itens.Add(vendaItem);
        }

        venda.Total = venda.Itens.Sum(i => i.Subtotal);

        _context.Vendas.Add(venda);
        await _context.SaveChangesAsync();

        return venda;
    }

    public async Task<Venda> AdicionarProdutoPorScannerAsync(ScannerVendaDto dto)
    {
        // 1️⃣ Buscar venda aberta
        var venda = await _context.Vendas
            .Include(v => v.Itens)
            .FirstOrDefaultAsync(v => v.Id == dto.VendaId && v.Status == "Aberta");

        if (venda == null)
            throw new Exception("Venda não encontrada ou já fechada.");

        // 2️⃣ Buscar produto pelo código (barras ou QR)
        var produto = await _context.Produtos
            .FirstOrDefaultAsync(p => p.CodigoBarras == dto.Codigo && p.Ativo);

        if (produto == null)
            throw new Exception("Produto não encontrado ou inativo.");

        // 3️⃣ Validar estoque
        if (produto.Estoque <= 0)
            throw new Exception("Produto sem estoque.");

        // 4️⃣ Verificar se já existe item na venda
        var item = venda.Itens.FirstOrDefault(i => i.ProdutoId == produto.Id);

        if (item != null)
        {
            item.Quantidade++;
        }
        else
        {
            venda.Itens.Add(new VendaItem
            {
                ProdutoId = produto.Id,
                Quantidade = 1,
                PrecoUnitario = produto.PrecoAtual
            });
        }

        // 5️⃣ Baixa automática no estoque
        produto.Estoque--;

        // 6️⃣ Recalcular total da venda
        venda.Total = venda.Itens.Sum(i => i.Quantidade * i.PrecoUnitario);

        await _context.SaveChangesAsync();
        return venda;
    }

    public async Task<List<Produto>> ProdutosComEstoqueBaixoAsync()
    {
        return await _context.Produtos
            .Where(p => p.Estoque <= p.EstoqueMinimo && p.Ativo)
            .ToListAsync();
    }

    public async Task<List<Produto>> ProdutosSemEstoqueAsync()
    {
        return await _context.Produtos
            .Where(p => p.Estoque == 0 && p.Ativo)
            .ToListAsync();
    }
    public async Task ReporEstoqueAsync(ReporEstoqueDto dto)
    {
        var produto = await _context.Produtos.FindAsync(dto.ProdutoId);
        if (produto == null) throw new Exception("Produto não encontrado.");

        produto.Estoque += dto.Quantidade;
        await _context.SaveChangesAsync();
    }

    public async Task<object> GiroEstoqueAsync()
    {
        return await _context.VendaItens
            .GroupBy(i => i.ProdutoId)
            .Select(g => new
            {
                ProdutoId = g.Key,
                QuantidadeVendida = g.Sum(i => i.Quantidade)
            })
            .OrderByDescending(x => x.QuantidadeVendida)
            .ToListAsync();
    }




}
