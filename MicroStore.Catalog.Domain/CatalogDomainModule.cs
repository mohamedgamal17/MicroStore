using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Domain.Configuration;
using Volo.Abp.Modularity;
namespace MicroStore.Catalog.Domain
{
    public class CatalogDomainModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();
            var appSettings =  config.Get<ApplicationSettings>();
            context.Services.AddSingleton(appSettings);
        }
    }
}
