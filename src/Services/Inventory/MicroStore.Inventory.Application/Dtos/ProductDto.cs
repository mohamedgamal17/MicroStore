#pragma warning disable CS8618
using Volo.Abp.Application.Dtos;
namespace MicroStore.Inventory.Application.Dtos
{
    public class ProductDto : EntityDto<string>
    {
        public int Stock { get;  set; }
    }
}
