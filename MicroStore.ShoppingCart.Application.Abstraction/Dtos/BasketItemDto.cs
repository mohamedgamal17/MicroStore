#nullable disable
namespace MicroStore.ShoppingCart.Application.Abstraction.Dtos
{
    public class BasketItemDto 
    {
        public Guid ItemId { get; set; }
        public ProductDTO Product { get; set; }
        public int Quantity { get; set; }
        
    }
}
