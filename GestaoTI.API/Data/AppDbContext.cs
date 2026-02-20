using GestaoTI.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GestaoTI.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Chamado> Chamados { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }


    }
}
