using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;

namespace MicroStore.Payment.Domain.Shared
{
    [DependsOn(typeof(AbpFluentValidationModule))]
    public class PaymentDomainSharedModule : AbpModule
    {

    }
}
