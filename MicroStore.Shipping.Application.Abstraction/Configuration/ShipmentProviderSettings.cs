namespace MicroStore.Shipping.Application.Abstraction.Configuration
{
    public class ShipmentProviderSettings : Dictionary<string, ShipmentProviderSetting>
    {
        public ShipmentProviderSetting FindByKey(string key)
        {

            if (TryGetValue(key, out var value))
            {
                return value;
            }

            return default;
        }
    }

    public class ShipmentProviderSetting
    {
        public string ApiKey { get; set; }
    }
}
