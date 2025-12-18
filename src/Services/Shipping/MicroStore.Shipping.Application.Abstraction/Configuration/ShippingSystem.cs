#pragma warning disable CS8618
namespace MicroStore.Shipping.Application.Abstraction.Configuration
{
    public class ShippingSystem
    {
        public string Name { get; private set; }
        public string DisplayName { get; private set; }
        public string Image { get; private set; }      
        public Type Provider { get;private set; }

        public bool IsEnabled => _configuration != null;
        public ShipmentProviderSetting Configuration
        {
            get
            {
                if (IsEnabled)
                {
                    return _configuration;
                }

                throw new InvalidOperationException($"Shipment system {Name} has not been configured");
            }
        }


        private readonly ShipmentProviderSetting? _configuration;

        public ShippingSystem(string name, string displayName, string image, Type provider, ShipmentProviderSetting configuration)
        {
            Name = name;
            DisplayName = displayName;
            Image = image;
            Provider = provider;
            _configuration = configuration;
        }

        public ShippingSystem()
        {
                
        }

        public static ShippingSystem Create(string name , string displayName, string image, Type provider, ShipmentProviderSetting? configuration)
        {
            return new ShippingSystem(name, displayName, image, provider, configuration);
        }

    }

    public class ShippingSystemOptions
    {
        public List<ShippingSystem> Systems { get; set; }

        public ShippingSystemOptions()
        {
                Systems = new List<ShippingSystem>();
        }
    }
}
