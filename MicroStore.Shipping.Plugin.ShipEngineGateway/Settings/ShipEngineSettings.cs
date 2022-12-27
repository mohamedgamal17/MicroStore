namespace MicroStore.Shipping.Plugin.ShipEngineGateway.Settings
{
    public class ShipEngineSettings
    {
        public string ApiKey { get; set; }
        public List<ShipEngineCarrierSettings> Carriers { get; set; }
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
