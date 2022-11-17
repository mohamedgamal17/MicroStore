#nullable disable
namespace MicroStore.Ordering.Application.Abstractions.Dtos
{
    public class PaymentDto
    {
        public string TransactionId { get; set; }
        public string Gateway { get; set; }

    }
}
