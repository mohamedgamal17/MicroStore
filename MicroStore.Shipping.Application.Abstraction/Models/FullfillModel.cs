using MicroStore.Shipping.Domain.ValueObjects;

namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class FullfillModel
    {
        public AddressModel AddressFrom { get; set; }
        public PackageModel Package {get; set;}

        public string CarrierId { get; set; }
    }
}
