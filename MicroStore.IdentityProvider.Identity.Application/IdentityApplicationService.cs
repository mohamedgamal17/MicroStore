using Volo.Abp.Application.Services;
using Volo.Abp.AutoMapper;

namespace MicroStore.IdentityProvider.Identity.Application
{
    public class IdentityApplicationService : ApplicationService
    {
        protected IMapperAccessor MapperAccessor => LazyServiceProvider.LazyGetRequiredService<IMapperAccessor>();
    }
}
