#nullable disable
using MicroStore.Payment.Domain.Shared;
using MicroStore.Payment.Plugin.StripeGateway.Consts;

namespace MicroStore.Payment.Plugin.StripeGateway.Config
{
    public class StripePaymentSettings : ISettings
    {
        public string ProviderKey => StripePaymentConst.Provider;
        public string ApiKey { get; set; }
        public string EndPointSecret { get; set; }
        public List<string> PaymentMethods { get; set; }
        public string Currency { get; set; }

    
    }
}
