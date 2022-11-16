#nullable disable
using MicroStore;

namespace MicroStore.Payment.Domain.Shared.Dtos
{
    public class PaymentStartedDto
    {
        public Guid PaymentId { get; set; }
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string TransctionId { get; set; }
        public string PaymentGateway { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? OpenedAt { get; set; }

    }
}
