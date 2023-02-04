using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.Identity.Application;
using Volo.Abp.Modularity;
namespace MicroStore.IdentityProvider.Identity.Infrastructure
{
    [DependsOn(typeof(IdentityApplicationModule))]
    public class IdentityInfrastructureModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();

            context.Services.AddDbContext<ApplicationIdentityDbContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"), sqlOpt =>
                {
                    sqlOpt.MigrationsAssembly(typeof(IdentityInfrastructureModule).Assembly.FullName);
                    sqlOpt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
                opt.EnableSensitiveDataLogging();
            },ServiceLifetime.Transient);
        }
    }
}
