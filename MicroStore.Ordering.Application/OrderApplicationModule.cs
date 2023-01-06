using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Security;
using MicroStore.Ordering.Application.Abstractions;
using Volo.Abp.Authorization;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
using Volo.Abp.Validation;
namespace MicroStore.Ordering.Application
{

    [DependsOn(typeof(MicroStoreSecurityModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpUnitOfWorkModule),
        typeof(InMemoryBusModule),
        typeof(AbpFluentValidationModule),
        typeof(OrderApplicationAbstractionModule))]
    public class OrderApplicationModule : AbpModule
    {

    }
}
