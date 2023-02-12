namespace MicroStore.ShoppingCart.Api.Models
{
    public class BasketModel
    {
        public List<BasketItemModel> Items { get; set; }

    }

    public class BasketItemModel
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class RemoveBasketItemsModel
    {
        public string[] ProductIds { get; set; }
    }
}
