using Volo.Abp.Application.Dtos;

namespace MicroStore.Shipping.Application.Abstraction.Dtos
{
    public class ShipmentItemDto : EntityDto<string>
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public string ProductId { get; set; }
        public string Thumbnail { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public WeightDto Weight { get; set; }
        public DimensionDto Dimension { get; set; }

        public ShipmentItemDto()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
