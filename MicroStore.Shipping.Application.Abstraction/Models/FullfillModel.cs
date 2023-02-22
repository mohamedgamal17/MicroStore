using MicroStore.Shipping.Domain.ValueObjects;

namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class FullfillModel
    {
        public string SystemName { get; set; }
        public AddressModel AddressFrom { get; set; }
        public AddressModel AddressTo { get; set; }
        public PackageModel Package {get; set;}
       
    }
}
