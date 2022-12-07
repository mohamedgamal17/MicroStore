#nullable disable
namespace MicroStore.Payment.Api.Models
{
    public class CompletePaymentRequestModel
    {
        public string PaymentGatewayName { get; set; }
        public string Token { get; set; }
    }
}
