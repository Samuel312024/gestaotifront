using integracoes.Controllers;
using integracoes.Models;
using Microsoft.EntityFrameworkCore;

namespace integracoes.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Adicione DbSets para suas entidades aqui (exemplos iniciais)
        //public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<ConsultasEnderecos> ConsultasEnderecos { get; set; }
        public DbSet<integracoes.Models.ConsultaPlaca> ConsultasPlacas { get; set; }
        public DbSet<RawDataDto> RawDatas { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações globais opcionais (ex.: case-insensitive para SQL Server)
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI");

            // Exemplo de configuração de tabela (opcional agora)
            modelBuilder.Entity<ConsultaPlaca>().HasNoKey();
        }
    }

    
}