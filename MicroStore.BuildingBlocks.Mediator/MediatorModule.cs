using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using Volo.Abp.Modularity;
namespace MicroStore.BuildingBlocks.Mediator
{
    [DependsOn(typeof(InMemoryBusModule))]
    public class MediatorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMediatR(Assembly.GetExecutingAssembly());

            context.Services.AddTransient(typeof(MediatR.IRequestHandler<,>), typeof(ReqeustHandlerAdapter<,>));

            context.Services.AddTransient<ILocalMessageBus, LocalMessageBus>();
        }
    }
}