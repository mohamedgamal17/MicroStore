namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing
{
    public class PaymentRequestProduct : BaseEntity<string>
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
