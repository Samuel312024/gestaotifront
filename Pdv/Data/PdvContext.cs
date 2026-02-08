using Integracoes.Pdv.Models;
using Microsoft.EntityFrameworkCore;
using Pdv.Models;
using System.Collections.Generic;

namespace Integracoes.Pdv.Data;

public class PdvContext : DbContext
{
    public PdvContext(DbContextOptions<PdvContext> options)
        : base(options) { }

    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Venda> Vendas => Set<Venda>();
    public DbSet<VendaItem> VendaItens => Set<VendaItem>();
    public DbSet<HistoricoPreco> HistoricoPrecos { get; set; }

}
