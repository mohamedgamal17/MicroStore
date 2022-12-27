﻿using MicroStore.BuildingBlocks.InMemoryBus;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;

namespace MicroStore.Shipping.Application.Abstraction
{
    [DependsOn(typeof(InMemoryBusModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpValidationModule))]
    public class ShippingApplicationAbstractionModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(cfg =>
            {
                cfg.AddMaps<ShippingApplicationAbstractionModule>();
            });

        }
    }
}