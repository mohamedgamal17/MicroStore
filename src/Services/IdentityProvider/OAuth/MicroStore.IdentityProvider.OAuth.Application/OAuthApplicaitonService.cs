using Volo.Abp.Application.Services;
using Volo.Abp.AutoMapper;

namespace MicroStore.IdentityProvider.OAuth.Application
{
    public abstract class OAuthApplicaitonService : ApplicationService
    {
        public IMapperAccessor MapperAccessor => LazyServiceProvider.LazyGetRequiredService<IMapperAccessor>();
    }
}
