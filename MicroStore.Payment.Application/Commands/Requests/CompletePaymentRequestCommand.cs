#nullable disable
namespace MicroStore.Payment.Application.Commands.Requests
{
    public class CompletePaymentRequestCommand
    {
        public Guid PaymentId { get; set; }
        public string PaymentGatewayName { get; set; }
    }
}
