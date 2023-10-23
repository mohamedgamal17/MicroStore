namespace MicroStore.Shipping.Application.Abstraction.Configuration
{
    public class ApplicationSettings
    {
        public SecuritySettings Security { get; set; } = new SecuritySettings();
        public MassTransitSettings MassTransit { get; set; } = new MassTransitSettings();
        public ConnectionStringSettings ConnectionStrings { get; set; } = new ConnectionStringSettings();
        public ShipmentProviderSettings ShipmentProviders { get; set; } = new ShipmentProviderSettings();
    }


}
