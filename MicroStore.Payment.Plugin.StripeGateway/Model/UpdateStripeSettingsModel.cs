namespace MicroStore.Payment.Plugin.StripeGateway.Model
{
    public class UpdateStripeSettingsModel  
    {
        public string ApiKey { get; set; }
        public string EndPointSecret { get; set; }
        public List<string> PaymentMethods { get; set; }
        public string Currency { get; set; }

    }
}
