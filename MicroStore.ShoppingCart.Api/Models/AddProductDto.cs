namespace MicroStore.ShoppingCart.Api.Models
{
    public class AddProductDto
    {
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
