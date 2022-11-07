#nullable disable
namespace MicroStore.ShoppingCart.Application.Abstraction.Dtos
{
    public class BasketDto 
    {
        public Guid BasketId { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<BasketItemDto> LineItems { get; set; }
        public decimal SubTotal { get; set; }

    }
}
