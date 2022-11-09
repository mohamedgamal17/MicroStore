#nullable disable
namespace MicroStore.Payment.Api.Dtos
{
    public class PaymentDto
    {
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
    }
}
