#nullable disable
using MicroStore;

namespace MicroStore.Ordering.Application.Dtos
{
    public class PaymentDto
    {
        public string TransactionId { get; set; }
        public string Gateway { get; set; }

    }
}
