

namespace MicroStore.ShoppingCart.Application.Abstraction.Dtos
{
    public class BasketDto 
    {
        public Guid BasketId { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<BasketItemDto> LineItems { get; set; }
        public decimal SubTotal { get; set; }

        public void Map(Profile profile)
        {
            profile.CreateMap<Basket, BasketDto>()
                .ForMember(x => x.BasketId, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.UserId, opt => opt.MapFrom(c => c.UserId))
                .ForMember(x => x.LineItems, opt => opt.MapFrom(c => c.LineItems));

        }
    }
}
