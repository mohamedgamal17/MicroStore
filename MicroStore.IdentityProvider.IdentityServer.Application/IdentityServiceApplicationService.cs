using Volo.Abp.Application.Services;
using Volo.Abp.AutoMapper;

namespace MicroStore.IdentityProvider.IdentityServer.Application
{
    public abstract class IdentityServiceApplicationService : ApplicationService
    {
        public IMapperAccessor MapperAccessor => LazyServiceProvider.LazyGetRequiredService<IMapperAccessor>();
    }
}
