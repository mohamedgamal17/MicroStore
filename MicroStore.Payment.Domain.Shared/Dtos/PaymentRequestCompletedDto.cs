#nullable disable
namespace MicroStore.Payment.Domain.Shared.Dtos
{
    public class PaymentRequestCompletedDto
    {
        public Guid PaymentId { get; set; }
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxCost { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TotalCost { get; set; }
        public string TransctionId { get; set; }
        public string PaymentGateway { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CapturedAt { get; set; }
    }
}
