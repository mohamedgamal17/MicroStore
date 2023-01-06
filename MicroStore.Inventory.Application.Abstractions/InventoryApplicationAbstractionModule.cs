using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Inventory.Application.Abstractions.Profiles;
using Volo.Abp.AutoMapper;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;
namespace MicroStore.Inventory.Application.Abstractions
{
    [DependsOn(typeof(InMemoryBusModule),
        typeof(AbpFluentValidationModule),
        typeof(AbpAutoMapperModule))]
    public class InventoryApplicationAbstractionModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(opt =>
            {
                opt.AddProfile<OrderProfile>();
                opt.AddProfile<ProductProfile>();
            });
        }
    }
}
