using MicroStore.Shipping.Application.Abstraction.Dtos;

namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class ShipmentItemEstimationModel
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public MoneyDto UnitPrice { get; set; }
        public int Quantity { get; set; }
        public WeightModel Weight { get; set; }
    }
}
