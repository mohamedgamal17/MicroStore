using Volo.Abp.Application.Services;
using Volo.Abp.AutoMapper;

namespace MicroStore.Payment.Application
{
    public class PaymentApplicationService : ApplicationService
    {
        protected IMapperAccessor MapperAccessor => LazyServiceProvider.LazyGetRequiredService<IMapperAccessor>();
    }
}
