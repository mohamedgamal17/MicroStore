using Volo.Abp.Modularity;
using Volo.Abp;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using System.Reflection;
using Volo.Abp.Autofac;
using MicroStore.Catalog.Infrastructure;
using MicroStore.BuildingBlocks.Mediator;

namespace MicroStore.Catalog.Application.Tests
{
    [DependsOn(typeof(CatalogApplicationModule))]
    [DependsOn(typeof(CatalogInfrastructureModule))]
    [DependsOn(typeof(AbpAutofacModule))]
    [DependsOn(typeof(MediatorModule))]
    public class CatalogApplicationTestModule : AbpModule
    {


        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMassTransitTestHarness(busRegisterConfig =>
            {
                busRegisterConfig.AddConsumers(Assembly.Load("MicroStore.Catalog.Application"));

                busRegisterConfig.UsingInMemory((context, inMemoryBusConfig) =>
                {
                    inMemoryBusConfig.ConfigureEndpoints(context);
                });
            });
        }

        public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

                await  dbContext.Database.MigrateAsync();
            }
        }


        public override async Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

                await dbContext.Database.EnsureDeletedAsync();
            }
        }


    }
}
