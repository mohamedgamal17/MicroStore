﻿using Microsoft.Extensions.DependencyInjection;
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

            Configure<AbpAutoMapperOptions>(opt =>
            {
                opt.AddProfile<AddressMappingProfile>();
                opt.AddProfile<ShipmentItemMappingProfile>();
            });
        }
   
    }
}
