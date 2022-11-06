
namespace MicroStore.Catalog.Application.Products.EventHandlers
{
    public class UpdateProductEvent
    {
        public Guid ProductId { get; private set; }
        public string Name { get; private set; }
        public string? Sku { get; private set; }
        public string? ShortDescription { get; private set; }
        public string? LongDescription { get; set; }

        public UpdateProductEvent(Guid productId, string name, string? sku, string? shortDescription, string? longDescription)
        {
            ProductId = productId;
            Name = name;
            Sku = sku;
            ShortDescription = shortDescription;
            LongDescription = longDescription;
        }
    }
}
