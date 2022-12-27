using MicroStore.Shipping.Plugin.ShipEngineGateway.Profiles;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Settings;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
namespace MicroStore.Shipping.Plugin.ShipEngineGateway
{
    public class ShipEngineSystemModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            ConfigureShipEngine();

            Configure<AbpAutoMapperOptions>(opt =>
            {
                opt.AddProfile<AddressMappingProfile>();
                opt.AddProfile<ShipmentItemMappingProfile>();
            });
        }


        private void ConfigureShipEngine()
        {
            Configure<ShipEngineSettings>(cfg =>
            {
                cfg.ApiKey = "TEST_sM8K2oczFz2bPZ5IcSQDGR3w1z5PuD7Cl6E5+hYxNM8";
                cfg.Carriers = new List<ShipEngineCarrierSettings>
                {
                    new ShipEngineCarrierSettings
                    {
                        CarrierId = "se-3687966",
                        DisplayName ="FeedEx",
                        Image ="none",
                        Name = "FeedEX",
                        IsEnabled =true
                    }
                };
            });

        }
    }
}
