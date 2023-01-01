using Microsoft.Extensions.DependencyInjection;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Settings;
using MicroStore.Shipping.PluginInMemoryTest;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace MicroStore.Shipping.Plugin.ShipEngineGateway.Tests
{
    [DependsOn(typeof(PluginInMemoryModule),
        typeof(ShipEngineSystemModule))]
    public class ShipEngineGatewayTestModule : AbpModule
    {

        public override async void OnApplicationInitialization(ApplicationInitializationContext context)
        {
           


        }

    }
}