using Volo.Abp.Application.Services;
using Volo.Abp.AutoMapper;
using Volo.Abp.Validation;

namespace MicroStore.Payment.Application
{
    [DisableValidation]
    public class PaymentApplicationService : ApplicationService
    {
        protected IMapperAccessor MapperAccessor => LazyServiceProvider.LazyGetRequiredService<IMapperAccessor>();
    }
}
