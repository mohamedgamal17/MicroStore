namespace MicroStore.Payment.IntegrationEvents.Models
{
    public class PaymentRequestProductModel
    {
        public string  ProductId { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
