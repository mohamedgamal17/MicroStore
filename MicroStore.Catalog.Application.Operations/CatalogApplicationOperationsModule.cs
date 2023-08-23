using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Domain;
using Volo.Abp.AutoMapper;
using Volo.Abp.EventBus;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;
namespace MicroStore.Catalog.Application.Operations
{
    [DependsOn(typeof(CatalogDomainModule),
        typeof(AbpEventBusModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpFluentValidationModule),
        typeof(CatalogApplicationModule))]
    public class CatalogApplicationOperationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();

            ConfigureHangFire(context.Services, config);

            Configure<AbpAutoMapperOptions>(opt => opt.AddMaps<CatalogApplicationOperationsModule>());
        }
        private void ConfigureHangFire(IServiceCollection services , IConfiguration configuration)
        {
            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));
            });
        }
    }
}