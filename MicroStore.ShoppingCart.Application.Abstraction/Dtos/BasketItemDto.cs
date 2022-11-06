

namespace MicroStore.ShoppingCart.Application.Abstraction.Dtos
{
    public class BasketItemDto 
    {
        public Guid ItemId { get; set; }
        public ProductDTO Product { get; set; }
        public int Quantity { get; set; }
        public void Map(Profile profile)
        {
            profile.CreateMap<BasketItem, BasketItemDto>()
                .ForMember(x => x.ItemId, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.Product, opt => opt.MapFrom(c => c.Product))
                .ForMember(x => x.Quantity, opt => opt.MapFrom(c => c.Quantity));

        }
    }
}
