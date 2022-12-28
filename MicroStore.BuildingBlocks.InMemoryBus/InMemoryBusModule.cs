using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.InMemoryBus.Behaviours;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.InMemoryBus.Piplines;
using MicroStore.BuildingBlocks.InMemoryBus.Wrappers;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
using Volo.Abp.Validation;

namespace MicroStore.BuildingBlocks.InMemoryBus
{
    [DependsOn(typeof(AbpUnitOfWorkModule),
    typeof(AbpAutoMapperModule),
        typeof(AbpValidationModule))]
    public class InMemoryBusModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddConventionalRegistrar(new RequestHandlerConventionalRegistar());
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient(typeof(RequestHandlerWrapperImpl<,>));
            context.Services.AddTransient(typeof(IRequestMiddleware<>), typeof(RequestPreProcessorBehavior<>));
            context.Services.AddTransient(typeof(IRequestMiddleware<>), typeof(RequestPostProcessorBehaviour<>));
            context.Services.AddTransient(typeof(IRequestMiddleware<>), typeof(ValidationBehaviour<>));
        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
          
                var contributorTypes = new List<Type>();

                context.Services.OnRegistred(context =>
                {
                    var validationInterceptor=  context.Interceptors
                        .SingleOrDefault(x => x == typeof(ValidationInterceptor));

                    if(validationInterceptor != null)
                    {
                        context.Interceptors.Remove(validationInterceptor);
                    }
                });

        }
    }
}
