using MicroStore.ShoppingGateway.ClinetSdk.Entities.Geographic;

namespace MicroStore.ShoppingGateway.ClinetSdk.Common
{
    public class AddressAggregate
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public Country Country { get; set; }
        public StateProvince State { get; set; }
        public string City { get; set; }

        public string PostalCode { get; set; }
        public string Zip { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
    }
}
