using Volo.Abp.Application.Services;
using Volo.Abp.AutoMapper;

namespace MicroStore.Geographic.Application
{
    public class GeographicApplicationService : ApplicationService
    {
        protected IMapperAccessor MapperAccessor => LazyServiceProvider.LazyGetRequiredService<IMapperAccessor>();
    }
}
