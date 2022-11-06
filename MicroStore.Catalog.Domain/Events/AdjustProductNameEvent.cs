namespace MicroStore.Catalog.Domain.Events
{
    public class AdjustProductNameEvent
    {
        public Guid ProductId { get; }

        public string Name { get; }

        public AdjustProductNameEvent(Guid productId, string name)
        {
            ProductId = productId;
            Name = name;
        }
    }
}
