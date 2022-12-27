using Microsoft.Extensions.DependencyInjection;
using MicroStore.Payment.PluginInMemoryTest;
using Stripe;
using Volo.Abp.Modularity;
using Microsoft.Extensions.Configuration;
namespace MicroStore.Payment.Plugin.StripeGateway.Tests
{
    [DependsOn(typeof(StripeGatewayPluginModule),
        typeof(PluginInMemoryModule))]
    public class StripGatewayTestModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //var config = context.Services.GetRequiredService<IConfiguration>();

            StripeConfiguration.ApiKey = "sk_test_51KykJuCNMS50bRuoRLb9mQno0ccoBllCtIKo7vL0QCxPMPhfpM9MyrqW8WiTWag0ujWCpqDCMDgvbUUTNYsWi2og00eREAe9Pu";

        }
    }
}
