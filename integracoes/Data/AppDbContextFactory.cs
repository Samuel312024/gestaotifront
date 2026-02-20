using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using integracoes.Data;

namespace integracoes.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseSqlServer(
            "Server=DESKTOP-2DVI25I,1433;" +
            "Database=IntegracaoDb;" +
            "User Id=Muka;" +
            "Password=202713AME;" +
            "TrustServerCertificate=True;",
            sqlOptions =>
                {
            sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        );
    }
);


            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
