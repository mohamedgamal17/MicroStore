using MicroStore.Payment.IntegrationEvents.Models;

namespace MicroStore.Payment.IntegrationEvents
{
    public class CreatePaymentRequestIntegrationEvent
    {
        public string  OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TaxCost { get; set; }
        public decimal TotalCost { get; set; }
        public List<PaymentRequestProductModel> Items { get; set; }
    }
}
