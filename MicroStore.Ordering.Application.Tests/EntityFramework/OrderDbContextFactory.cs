using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MicroStore.Ordering.Infrastructure.EntityFramework;

namespace MicroStore.Ordering.Application.Tests.EntityFramework
{
    public class OrderDbContextFactory : IDesignTimeDbContextFactory<OrderDbContext>
    {
        public OrderDbContext CreateDbContext(string[] args)
        {
            var config = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<OrderDbContext>()
                .UseSqlServer(config.GetConnectionString("DefaultConnection"), (opt) =>
                {
                    opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    opt.MigrationsAssembly(typeof(OrderDbContext).Assembly.FullName);
                });


            return new OrderDbContext(builder.Options);
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
