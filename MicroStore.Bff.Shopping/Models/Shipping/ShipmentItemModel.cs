using MicroStore.Bff.Shopping.Models.Common;

namespace MicroStore.Bff.Shopping.Models.Shipping
{
    public class ShipmentItemModel
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public string ProductId { get; set; }
        public string Thumbnail { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public WeightModel Weight { get; set; }
        public DimensionModel Dimension { get; set; }

        public ShipmentItemModel()
        {
            Name = string.Empty;
            Sku = string.Empty;
            ProductId = string.Empty;
            Thumbnail = string.Empty;
            Quantity = 0;
            UnitPrice = 0;
            Weight = new WeightModel();
            Dimension = new DimensionModel();
        }
    }
}
