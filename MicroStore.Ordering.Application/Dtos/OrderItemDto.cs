using Volo.Abp.Application.Dtos;

namespace MicroStore.Ordering.Application.Dtos
{
    public class OrderItemDto : EntityDto<Guid>
    {
        public string ExternalProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
