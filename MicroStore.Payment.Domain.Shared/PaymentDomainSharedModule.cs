using MicroStore.Payment.Application.Dtos;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace MicroStore.Payment.Domain.Shared
{
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class PaymentDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(opt =>
            {
                opt.AddProfile<PaymentProfile>(false);
            });
        }

    }
}
