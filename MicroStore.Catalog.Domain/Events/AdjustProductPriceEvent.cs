namespace MicroStore.Catalog.Domain.Events
{
    public class AdjustProductPriceEvent
    {

        public Guid ProductId { get; set; }
        public decimal Price { get; set; }

        public AdjustProductPriceEvent(Guid productid, decimal price)
        {
            ProductId = productid;
            Price = price;
        }

    }
}
