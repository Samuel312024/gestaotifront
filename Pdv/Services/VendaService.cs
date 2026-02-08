using Integracoes.Pdv.Data;
using Integracoes.Pdv.Models;
using Integracoes.Pdv.DTOs;
using Microsoft.EntityFrameworkCore;

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
}
