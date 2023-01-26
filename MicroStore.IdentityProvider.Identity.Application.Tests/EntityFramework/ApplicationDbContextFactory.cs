using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MicroStore.IdentityProvider.Identity.Infrastructure;
namespace MicroStore.IdentityProvider.Identity.Application.Tests.EntityFramework
{
    internal class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationIdentityDbContext>
    {
        public ApplicationIdentityDbContext CreateDbContext(string[] args)
        {
            var config = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<ApplicationIdentityDbContext>()
                .UseSqlServer(config.GetConnectionString("DefaultConnection"), (opt) =>
                {
                    opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    opt.MigrationsAssembly(typeof(ApplicationIdentityDbContext).Assembly.FullName);
                });


            return new ApplicationIdentityDbContext(builder.Options);
        }
        private IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../MicroStore.IdentityProvider.Identity.Application.Tests/"))
                .AddJsonFile("appsettings.json", false)
                .Build();
        }
    }
}
