using MicroStore.BuildingBlocks.InMemoryBus;
using System.Reflection;
using Volo.Abp.AutoMapper;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;

namespace MicroStore.Ordering.Application.Abstractions
{
    [DependsOn(typeof(AbpAutoMapperModule),
        typeof(InMemoryBusModule),
        typeof(AbpFluentValidationModule))]
    public class OrderApplicationAbstractionModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            Configure<AbpAutoMapperOptions>(opt =>
            {
                opt.AddMaps<OrderApplicationAbstractionModule>();
            });
        }
    }
}