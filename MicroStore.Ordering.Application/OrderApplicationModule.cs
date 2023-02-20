using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Security;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.AutoMapper;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
namespace MicroStore.Ordering.Application
{

    [DependsOn(typeof(MicroStoreSecurityModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpUnitOfWorkModule),
        typeof(InMemoryBusModule),
        typeof(AbpFluentValidationModule),
        typeof(AbpDddApplicationModule))]
    public class OrderApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(opt => opt.AddMaps<OrderApplicationModule>());
        }
    }
}
