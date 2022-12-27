using MicroStore.Payment.IntegrationEvents.Models;

namespace MicroStore.Payment.IntegrationEvents
{
    public class CreatePaymentRequestIntegrationEvent
    {
        public string  OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public double SubTotal { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double TotalCost { get; set; }
        public List<PaymentRequestProductModel> Items { get; set; }
    }
}
