using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;
using MicroStore.IdentityProvider.IdentityServer.Application;
using MicroStore.IdentityProvider.IdentityServer.Application.Common;
using MicroStore.IdentityProvider.IdentityServer.Infrastructure.EntityFramework;
using MicroStore.IdentityProvider.IdentityServer.Infrastructure.Services;
using Volo.Abp.Modularity;

namespace MicroStore.IdentityProvider.IdentityServer.Infrastructure
{
    [DependsOn(typeof(IdentityServerApplicationModule))]
    public class IdentityServerInfrastrcutreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            context.Services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
            })
             .AddServerSideSessions()
             .AddConfigurationStore<ApplicationConfigurationDbContext>(cfg =>
             {
                 cfg.DefaultSchema = IdentityServerDbConsts.ConfigurationSchema;

                 cfg.ConfigureDbContext = (builder) =>
                 {
                     builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlServerOpt =>
                     {
                         sqlServerOpt.MigrationsAssembly(typeof(IdentityServerInfrastrcutreModule).Assembly.FullName)
                             .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

                     });


                 };
             })
             .AddOperationalStore<ApplicationPersistedGrantDbContext>(cfg =>
             {
                 cfg.DefaultSchema = IdentityServerDbConsts.OperationalSchema;

                 cfg.ConfigureDbContext = (builder) =>
                 {
                     builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlServerOpt =>
                     {
                         sqlServerOpt.MigrationsAssembly(typeof(IdentityServerInfrastrcutreModule).Assembly.FullName)
                             .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

                     });
                 };
             }).AddAspNetIdentity<ApplicationIdentityUser>()
             .AddExtensionGrantValidator<TokenExchangeExtensionGrantValidator>()
             .AddProfileService<ApplicationProfileService>();

            context.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}