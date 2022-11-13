using MicroStore.BuildingBlocks.InMemoryBus;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
namespace MicroStore.Inventory.Application.Abstractions
{
    [DependsOn(typeof(InMemoryBusModule),
        typeof(AbpValidationModule),
        typeof(AbpAutoMapperModule))]
    public class InventoryAbstractionModule : AbpModule
    {

    }
}
