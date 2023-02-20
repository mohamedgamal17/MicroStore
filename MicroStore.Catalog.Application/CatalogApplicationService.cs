using Volo.Abp.Application.Services;
using Volo.Abp.AutoMapper;

namespace MicroStore.Catalog.Application
{
    public class CatalogApplicationService : ApplicationService
    {
        protected IMapperAccessor MapperAccessor => LazyServiceProvider.LazyGetRequiredService<IMapperAccessor>();

    }
}
