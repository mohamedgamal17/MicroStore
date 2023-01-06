#nullable disable
using MicroStore;

namespace MicroStore.Payment.Api.Models.PaymentRequests
{
    public class ProcessPaymentRequestModel
    {
        public string PaymentGatewayName { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}
