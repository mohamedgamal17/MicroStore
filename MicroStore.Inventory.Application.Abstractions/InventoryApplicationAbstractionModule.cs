using MicroStore.BuildingBlocks.InMemoryBus;
using Volo.Abp.AutoMapper;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
namespace MicroStore.Inventory.Application.Abstractions
{
    [DependsOn(typeof(InMemoryBusModule),
        typeof(AbpFluentValidationModule),
        typeof(AbpAutoMapperModule))]
    public class InventoryApplicationAbstractionModule : AbpModule
    {

    }
}
