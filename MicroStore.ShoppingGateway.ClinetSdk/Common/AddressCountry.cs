using MicroStore.ShoppingGateway.ClinetSdk.Entities;

namespace MicroStore.ShoppingGateway.ClinetSdk.Common
{
    public class AddressCountry : BaseEntity<string>
    {
        public string Name { get; set; }
        public string TwoLetterIsoCode { get; set; }
        public string ThreeLetterIsoCode { get; set; }
        public int NumericIsoCode { get; set; }
    }
}
