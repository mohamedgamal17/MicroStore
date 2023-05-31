using MicroStore.ShoppingGateway.ClinetSdk.Common;

namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Cart
{
    public class BasketAggregate
    {
        public string UserName { get; set; }

        public List<BasketItemAggregate> Items { get; set; }
    }
    public class BasketItemAggregate
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string? Thumbnail { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public double Price { get; set; }
        public Weight Weight { get; set; }
        public Dimension Dimensions { get; set; }
        public int Quantity { get; set; }
    }
}
