using Volo.Abp.Application.Services;
using Volo.Abp.AutoMapper;
using Volo.Abp.Validation;

namespace MicroStore.Inventory.Application
{
    [DisableValidation]
    public class InventoryApplicationService : ApplicationService
    {
        protected IMapperAccessor MapperAccessor => LazyServiceProvider.LazyGetRequiredService<IMapperAccessor>();

    }
}
