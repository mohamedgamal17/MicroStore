using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Catalog.Application.Abstractions.Categories.Dtos;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using Volo.Abp.AutoMapper;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;

namespace MicroStore.Catalog.Application.Abstractions
{

    [DependsOn(typeof(InMemoryBusModule),
      typeof(AbpAutoMapperModule),
      typeof(AbpFluentValidationModule))]
    public class CatalogApplicationAbstractionModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(opt =>
            {
                opt.AddProfile<ProductProfile>(false);
                opt.AddProfile<CategoryProfile>(false);

            });
        }

    }
}