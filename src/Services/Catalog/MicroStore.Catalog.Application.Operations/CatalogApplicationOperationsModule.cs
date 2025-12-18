using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Operations.Recommandations;
using MicroStore.Catalog.Domain;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundJobs.Hangfire;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.EventBus;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;
namespace MicroStore.Catalog.Application.Operations
{
    [DependsOn(typeof(CatalogDomainModule),
        typeof(AbpEventBusModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpFluentValidationModule),
        typeof(AbpBackgroundJobsHangfireModule),
        typeof(CatalogApplicationModule))]
    public class CatalogApplicationOperationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();

            ConfigureHangFire(context.Services, config);

            Configure<AbpAutoMapperOptions>(opt => opt.AddMaps<CatalogApplicationOperationsModule>());

            Configure<AbpBackgroundJobOptions>(opt => opt.IsJobExecutionEnabled = true);    
        }
        private void ConfigureHangFire(IServiceCollection services , IConfiguration configuration)
        {
            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));
            });
        }

        public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
             await context.AddBackgroundWorkerAsync<MatrixFactorizationSynchronizationWorker>();
        }
    }
}