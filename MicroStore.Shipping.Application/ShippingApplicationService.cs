using Volo.Abp.Application.Services;
using Volo.Abp.AutoMapper;

namespace MicroStore.Shipping.Application
{
    public abstract class ShippingApplicationService : ApplicationService
    {
        protected IMapperAccessor MapperAccessor => LazyServiceProvider.LazyGetRequiredService<IMapperAccessor>();
    }
}
