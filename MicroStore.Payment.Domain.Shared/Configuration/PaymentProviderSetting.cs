namespace MicroStore.Payment.Domain.Shared.Configuration
{
    public class PaymentProviderSettings : Dictionary<string, PaymentProviderSetting>
    {

        public PaymentProviderSetting? FindByKey(string key)
        {
            if (TryGetValue(key, out var value))
            {
                return value;
            }

            return default;
        }
    }

    public class PaymentProviderSetting
    {
        public string ApiKey { get; set; }
        public string WebHookSecret { get; set; }
        public List<string> PaymentMethods { get; set; }
        public string Currency { get; set; }
    }
}
