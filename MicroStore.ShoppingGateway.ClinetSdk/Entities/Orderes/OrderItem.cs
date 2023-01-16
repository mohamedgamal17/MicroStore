namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes
{
    public class OrderItem : BaseEntity<Guid>
    {
        public string ExternalProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }

    }
}
