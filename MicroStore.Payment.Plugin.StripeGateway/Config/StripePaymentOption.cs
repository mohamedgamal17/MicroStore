#nullable disable
namespace MicroStore.Payment.Plugin.StripeGateway.Config
{
    public class StripePaymentOption
    {
        public string ApiKey { get; set; }
        public string EndPointSecret { get; set; }
        public List<string> PaymentMethods { get; set; }
        public string Currency { get; set; }

    }
}
