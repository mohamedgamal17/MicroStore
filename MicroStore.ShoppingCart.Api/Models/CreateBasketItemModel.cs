namespace MicroStore.ShoppingCart.Api.Models
{
    public class CreateBasketItemModel
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
