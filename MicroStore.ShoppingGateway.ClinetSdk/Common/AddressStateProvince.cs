using MicroStore.ShoppingGateway.ClinetSdk.Entities;

namespace MicroStore.ShoppingGateway.ClinetSdk.Common
{
    public class AddressStateProvince : BaseEntity<string>
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string CountryId { get; set; }
    }
}
