using MicroStore.Catalog.IntegrationEvents.Models;

namespace MicroStore.Catalog.IntegrationEvents
{
    public class ProductCreatedIntegrationEvent
    {
        public string ProductId { get;  set; }
        public string Sku { get; set; }
        public string Name { get;  set; }
        public string Description { get; set; }
        public double Price { get; set; }    
        public List<ProductImageModel> ProductImages { get; set; }
    }
}
