
using MicroStore.BuildingBlocks.Security;
using MicroStore.Ordering.Application.Abstractions;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace MicroStore.Ordering.Application
{

    [DependsOn(typeof(MicroStoreSecurityModule),
        typeof(AbpAuthorizationModule),
        typeof(OrderApplicationAbstractionModule))]
    public class OrderApplicationModule : AbpModule
    {

    }
}
