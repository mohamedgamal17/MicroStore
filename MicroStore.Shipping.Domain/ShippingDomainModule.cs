using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Shipping.Domain.Configuration;
using Volo.Abp.Modularity;

namespace MicroStore.Shipping.Domain
{
    public class ShippingDomainModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();

             var appsettings = config.Get<ApplicationSettings>();

            context.Services.AddSingleton(appsettings);
        }
    }
}
