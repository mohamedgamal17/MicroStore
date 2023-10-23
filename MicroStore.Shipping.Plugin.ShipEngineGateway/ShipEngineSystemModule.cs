using Microsoft.Extensions.DependencyInjection;
using MicroStore.Shipping.Application.Abstraction.Configuration;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Consts;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Profiles;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
namespace MicroStore.Shipping.Plugin.ShipEngineGateway
{
    public class ShipEngineSystemModule : AbpModule
    {
   
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(ShipEngineSystemModule).Assembly);
            });


            var appsettings = context.Services.GetSingletonInstance<ApplicationSettings>();

            Configure<ShippingSystemOptions>(opt =>
            {
                var systemConfig = appsettings.ShipmentProviders.FindByKey(ShipEngineConst.Provider);

                var system = ShippingSystem.Create(ShipEngineConst.Provider, ShipEngineConst.DisplayName, ShipEngineConst.Image, typeof(ShipEngineSystemProvider), systemConfig);

                opt.Systems.Add(system);
            });

            Configure<AbpAutoMapperOptions>(opt =>
            {
                opt.AddProfile<AddressMappingProfile>();
                opt.AddProfile<ShipmentItemMappingProfile>();
            });
        }




    }
}
