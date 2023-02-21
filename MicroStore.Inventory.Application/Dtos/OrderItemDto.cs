#pragma warning disable CS8618
using Volo.Abp.Application.Dtos;
namespace MicroStore.Inventory.Application.Dtos
{
    public class OrderItemDto : EntityDto<string>
    {

        public string ProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
