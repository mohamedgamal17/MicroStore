#nullable disable
using MicroStore;

namespace MicroStore.Payment.Api.Models.PaymentRequests
{
    public class CompletePaymentRequestModel
    {
        public string PaymentGatewayName { get; set; }
        public string Token { get; set; }
    }
}
