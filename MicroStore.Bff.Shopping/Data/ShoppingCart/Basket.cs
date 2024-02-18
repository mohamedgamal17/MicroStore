using MicroStore.Bff.Shopping.Data.Catalog;

namespace MicroStore.Bff.Shopping.Data.ShoppingCart
{
    public class Basket : Entity<string>
    {
        public string UserId { get; set; }
        public List<BasketItem> Items { get; set; }
    }


    public class BasketItem : Entity<string>
    {
        public string ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
