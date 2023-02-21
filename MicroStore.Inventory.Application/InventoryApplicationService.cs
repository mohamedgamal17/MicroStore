using Volo.Abp.Application.Services;
using Volo.Abp.AutoMapper;

namespace MicroStore.Inventory.Application
{
    public class InventoryApplicationService : ApplicationService
    {
        protected IMapperAccessor MapperAccessor => LazyServiceProvider.LazyGetRequiredService<IMapperAccessor>();

    }
}
