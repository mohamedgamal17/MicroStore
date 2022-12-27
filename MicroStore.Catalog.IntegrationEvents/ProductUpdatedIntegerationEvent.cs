namespace MicroStore.Catalog.IntegrationEvents
{
    public class ProductUpdatedIntegerationEvent
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public double UnitPrice { get; set; }
    }
}
