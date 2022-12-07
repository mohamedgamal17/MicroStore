#nullable disable
namespace MicroStore.Payment.Api.Models
{
    public class ProcessPaymentRequestModel
    {
        public string PaymentGatewayName { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}
