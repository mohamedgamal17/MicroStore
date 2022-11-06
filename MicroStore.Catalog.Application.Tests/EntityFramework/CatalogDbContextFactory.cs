
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MicroStore.Catalog.Infrastructure.EntityFramework;

namespace MicroStore.Catalog.Application.Tests.EntityFramework
{
    internal class CatalogDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext(string[] args)
        {
            var config = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseSqlServer(config.GetConnectionString("DefaultConnection"), (opt) =>
                {
                    opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    opt.MigrationsAssembly(typeof(CatalogDbContext).Assembly.FullName);
                });


            return new CatalogDbContext(builder.Options);
        }
        private IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../MicroStore.Ordering.Application.Tests/"))
                .AddJsonFile("appsettings.json", false)
                .Build();
        }
    }
}
