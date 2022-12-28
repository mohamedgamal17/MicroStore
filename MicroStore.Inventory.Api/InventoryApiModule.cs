using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.Mediator;
using MicroStore.Inventory.Application;
using MicroStore.Inventory.Infrastructure;
using Volo.Abp.Modularity;

namespace MicroStore.Inventory.Api
{
    [DependsOn(typeof(InventoryInfrastructureModule),
        typeof(InventoryApplicationModule),
         typeof(MediatorModule))]
    public class InventoryApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(InventoryApiModule).Assembly);
            });
        }
    }
}