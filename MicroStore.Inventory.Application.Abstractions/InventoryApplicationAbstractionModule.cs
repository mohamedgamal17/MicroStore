using MicroStore.BuildingBlocks.InMemoryBus;
using Volo.Abp.Modularity;

namespace MicroStore.Inventory.Application.Abstractions
{
    [DependsOn(typeof(InMemoryBusModule))]
    public class InventoryApplicationAbstractionModule : AbpModule
    {

    }
}