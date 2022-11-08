
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Security;
using MicroStore.Ordering.Application.Abstractions;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace MicroStore.Ordering.Application
{

    [DependsOn(typeof(MicroStoreSecurityModule),
        typeof(AbpAuthorizationModule),
        typeof(OrderApplicationAbstractionModule),
        typeof(AbpUnitOfWorkModule),
        typeof(InMemoryBusModule))]
    public class OrderApplicationModule : AbpModule
    {

    }
}
