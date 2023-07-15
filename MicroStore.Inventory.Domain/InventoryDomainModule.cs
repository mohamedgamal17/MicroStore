using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Inventory.Domain.Configuration;
using Volo.Abp.Modularity;
namespace MicroStore.Inventory.Domain
{
    public class InventoryDomainModule :AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();
            var appsettings = config.Get<ApplicationSettings>();
            context.Services.AddSingleton(appsettings);
        }
    }
}
