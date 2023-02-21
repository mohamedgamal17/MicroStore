#pragma warning disable CS8618
using Volo.Abp.Application.Dtos;
namespace MicroStore.Inventory.Application.Dtos
{
    public class ProductDto : EntityDto<string>
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public int Stock { get; private set; }
        public int AllocatedStock { get; private set; }
    }
}
