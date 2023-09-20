using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MicroStore.Profiling.Application.EntityFramewrok;

namespace MicroStore.Geographic.Application.Tests.EntityFramework
{
    internal class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var config = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(config.GetConnectionString("DefaultConnection"), (opt) =>
                {
                    opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    opt.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                });


            return new ApplicationDbContext(builder.Options);
        }
        private IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../MicroStore.Geographic.Application.Tests/"))
                .AddJsonFile("appsettings.json", false)
                .Build();
        }
    }
}
