#nullable disable
using MicroStore;
namespace MicroStore.Payment.Application.Abstractions.Dtos
{
    public class PaymentProcessResultDto
    {
        public string SessionId { get; set; }
        public string TransactionId { get; set; }
        public double AmountSubTotal { get; set; }
        public double AmountTotal { get; set; }
        public string CheckoutLink { get; set; }
        public string CancelUrl { get; set; }
        public string SuccessUrl { get; set; }

    }
}
