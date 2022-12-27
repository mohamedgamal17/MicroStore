#nullable disable
using MicroStore;

namespace MicroStore.Payment.Application.Abstractions.Dtos
{
    public class PaymentRequestCreatedDto
    {
        public Guid PaymentId { get; set; }
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxCost { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TotalCost { get; set; }
        public List<PaymentRequestProductDto> Items { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
