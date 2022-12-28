using MicroStore.Shipping.Domain.Common;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Consts;

namespace MicroStore.Shipping.Plugin.ShipEngineGateway.Settings
{
    public class ShipEngineSettings : ISettings
    {
        public string ProviderKey => ShipEngineConst.SystemName;
        public string ApiKey { get; set; } = string.Empty;
        public List<ShipEngineCarrierSettings> Carriers { get; set; } = new List<ShipEngineCarrierSettings>();

    }


    public class ShipEngineCarrierSettings
    {
        public string CarrierId { get; set; }
        public string Name{ get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public bool IsEnabled { get; set; }
    }
}
