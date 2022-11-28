namespace MicroStore.ShoppingCart.Api.Models
{
    public class BasketDto
    {
        public string UserId { get; set; }

        public List<BasketItemDto> Items { get; set; }
    }
}
