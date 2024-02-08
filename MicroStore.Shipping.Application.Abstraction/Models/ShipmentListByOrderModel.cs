namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class ShipmentListByOrderIdModel
    {
        public List<string> OrderIds { get; set; } = new List<string>();
    }

    public class ShipmentListByOrderNumberModel
    {
        public List<string> OrderNumbers { get; set; } = new List<string>();
    }
}
