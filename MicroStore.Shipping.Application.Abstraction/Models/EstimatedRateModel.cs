using MicroStore.Shipping.Domain.ValueObjects;

namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class EstimatedRateModel
    {
        public AddressModel AddressFrom { get; set; }
        public AddressModel AddressTo { get; set; }
        public List<ShipmentItemEstimationModel> Items { get; set; }
    }
}
