using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Entites;
using MicroStore.IdentityProvider.OAuth.Application;
using MicroStore.IdentityProvider.OAuth.Application.Common;
using MicroStore.IdentityProvider.OAuth.Infrastructure.EntityFramework;
using MicroStore.IdentityProvider.OAuth.Infrastructure.Services;
using Volo.Abp.Modularity;

namespace MicroStore.IdentityProvider.OAuth.Infrastructure
{
    [DependsOn(typeof(OAuthApplicationModule))]
    public class OAuthInfrastrcutreModule : AbpModule
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
                         sqlServerOpt.MigrationsAssembly(typeof(OAuthInfrastrcutreModule).Assembly.FullName)
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
                         sqlServerOpt.MigrationsAssembly(typeof(OAuthInfrastrcutreModule).Assembly.FullName)
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