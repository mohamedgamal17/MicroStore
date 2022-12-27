using MicroStore.Shipping.Domain.Common;
using MicroStore.Shipping.Domain.Const;

namespace MicroStore.Shipping.Domain.Entities
{
    public class ShippingSettings : ISettings
    {

        public string ProviderKey { get; private set; } = SettingsConst.ProviderKey;

        public AddressSettings Location { get; set; } = new AddressSettings();
        public bool ProductionMode { get; set; }
        
    }

    public class AddressSettings
    {
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Zip { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
    }

}
