using Volo.Abp.Application.Services;
using Volo.Abp.AutoMapper;
using Volo.Abp.Validation;
namespace MicroStore.Ordering.Application
{
    [DisableValidation]
    public abstract class OrderApplicationService : ApplicationService
    {
        protected IMapperAccessor MapperAccessor => LazyServiceProvider.LazyGetRequiredService<IMapperAccessor>();

    }
}
