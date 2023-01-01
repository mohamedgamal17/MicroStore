using MicroStore.Shipping.Domain.Common;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Consts;

namespace MicroStore.Shipping.Plugin.ShipEngineGateway.Settings
{
    public class ShipEngineSettings : ISettings
    {
        public string ProviderKey => ShipEngineConst.SystemName;
        public string ApiKey { get; set; } = string.Empty;

    } 
}
