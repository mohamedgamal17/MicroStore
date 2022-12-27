﻿using MicroStore.Payment.Application.Abstractions.Profiles;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace MicroStore.Payment.Application.Abstractions
{
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class PaymentApplicationAbstractionModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(opt =>
            {
                opt.AddProfile<PaymentProfile>(false);
                opt.AddProfile<PaymentSystemProfile>(false);
            });
        }

    }
}