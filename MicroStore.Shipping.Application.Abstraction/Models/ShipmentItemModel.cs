using MicroStore.Shipping.Domain.Entities;

namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class ShipmentItemModel
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public string ProductId { get; set; }
        public string Thumbnail { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public WeightModel Weight { get; set; }
        public DimensionModel Dimension { get; set; }
        public ShipmentItem AsShipmentItem()
        {
            return new ShipmentItem
            {
                Name = Name,
                Sku = Sku,
                ProductId = ProductId,
                Thumbnail = Thumbnail,
                Quantity = Quantity,
                UnitPrice = UnitPrice,
                Weight = Weight.AsWeight(),
                Dimension = Dimension.AsDimension()
            };
        }
    }
}
