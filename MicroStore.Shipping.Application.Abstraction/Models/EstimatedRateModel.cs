using MicroStore.Shipping.Domain.ValueObjects;

namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class EstimatedRateModel
    {
        public AddressModel Address { get; set; }
        public List<ShipmentItemEstimationModel> Items { get; set; }
    }
}
