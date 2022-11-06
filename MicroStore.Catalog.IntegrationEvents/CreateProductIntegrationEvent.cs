namespace MicroStore.Catalog.IntegrationEvents
{
    public class CreateProductIntegrationEvent
    {
        public Guid ProductId { get; private set; }
        public string Name { get; private set; }
        public string Sku { get; set; }
        public decimal Price { get; set; }

        public CreateProductIntegrationEvent(Guid productId, string name, string sku, decimal price)
        {
            ProductId = productId;
            Name = name;
            Sku = sku;
            Price = price;
        }
    }
}
