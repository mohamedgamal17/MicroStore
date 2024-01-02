using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Domain;
using Volo.Abp.AutoMapper;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;
namespace MicroStore.Catalog.Application.Abstractions
{
    [DependsOn(
        typeof(CatalogDomainModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpFluentValidationModule))]
    public class CatalogApplicationAbstractionModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            Configure<AbpAutoMapperOptions>(opt =>
            {
                opt.AddMaps<CatalogApplicationAbstractionModule>();

            });


            var configuration = context.Services.GetConfiguration();
        }
    }
}
