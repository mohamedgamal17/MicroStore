using MicroStore.BuildingBlocks.InMemoryBus;
using Volo.Abp.AutoMapper;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;

namespace MicroStore.ShoppingCart.Application.Abstraction
{
    [DependsOn(typeof(AbpAutoMapperModule),
        typeof(InMemoryBusModule),
        typeof(AbpFluentValidationModule))]
    public class ShoppingCartApplicationAbstractionModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(opt =>
            {
                opt.AddProfile<BasketProfile>(false);

            });

        }
    }
}