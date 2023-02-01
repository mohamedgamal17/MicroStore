using Duende.IdentityServer.EntityFramework.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.IdentityServer.Application;
using MicroStore.IdentityProvider.IdentityServer.Infrastructure.EntityFramework;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace MicroStore.IdentityProvider.IdentityServer.Infrastructure
{
    [DependsOn(typeof(IdentityServerApplicationModule))]
    public class IdentityServerInfrastrcutreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();

            context.Services.AddConfigurationDbContext<ApplicationConfigurationDbContext>(cfg =>
            {
                cfg.DefaultSchema = IdentityServerDbConsts.ConfigurationSchema;

                cfg.ConfigureDbContext= (builder) =>
                {
                    builder.UseSqlServer(config.GetConnectionString("DefaultConnection"), sqlServerOpt =>
                    {
                        sqlServerOpt.MigrationsAssembly(typeof(IdentityServerInfrastrcutreModule).Assembly.FullName)
                            .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

                    });


                };
            });

            var migrationAssembly = typeof(IdentityServerInfrastrcutreModule).Assembly.FullName;

            context.Services.AddOperationalDbContext<ApplicationPersistedGrantDbContext>(cfg =>
            {
               
                cfg.DefaultSchema = IdentityServerDbConsts.OperationalSchema;

                cfg.ConfigureDbContext = (builder) =>
                {
                    builder.UseSqlServer(config.GetConnectionString("DefaultConnection"), sqlServerOpt =>
                    {
                        sqlServerOpt.MigrationsAssembly(typeof(IdentityServerInfrastrcutreModule).Assembly.FullName)
                            .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

                    });
                };
            });
        }
    }
}