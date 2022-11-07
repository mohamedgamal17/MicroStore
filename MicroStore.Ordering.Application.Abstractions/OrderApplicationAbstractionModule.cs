using MicroStore.BuildingBlocks.InMemoryBus;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace MicroStore.Ordering.Application.Abstractions
{
    [DependsOn(typeof(AbpAutoMapperModule),
        typeof(InMemoryBusModule))]
    public class OrderApplicationAbstractionModule : AbpModule
    {

    }
}