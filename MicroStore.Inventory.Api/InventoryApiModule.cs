using Microsoft.Extensions.DependencyInjection;
using MicroStore.Inventory.Application;
using MicroStore.Inventory.Infrastructure;
using Volo.Abp.Modularity;

namespace MicroStore.Inventory.Api
{
    [DependsOn(typeof(InventoryInfrastructureModule),
        typeof(InventoryApplicationModule))]
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