using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MicroStore.Inventory.Infrastructure.EntityFramework;

namespace MicroStore.Inventory.Application.Tests.EntityFramework
{
    public class InventoryDbContextFactory : IDesignTimeDbContextFactory<InventoyDbContext>
    {
        public InventoyDbContext CreateDbContext(string[] args)
        {
            var config = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<InventoyDbContext>()
                .UseSqlServer(config.GetConnectionString("DefaultConnection")!, (opt) =>
                {
                    opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    opt.MigrationsAssembly(typeof(InventoyDbContext).Assembly.FullName);
                });


            return new InventoyDbContext(builder.Options);
        }

        private IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../MicroStore.Inventory.Application.Tests/"))
                .AddJsonFile("appsettings.json", false)
                .Build();
        }
    }
}
