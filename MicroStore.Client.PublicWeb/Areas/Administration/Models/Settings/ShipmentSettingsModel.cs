using MicroStore.Client.PublicWeb.Areas.Administration.Models.Common;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Settings
{
    public class ShipmentSettingsModel
    {
        public string DefaultShippingSystem { get; set; }

        public AddressModel Location { get; set; }
    }
}
