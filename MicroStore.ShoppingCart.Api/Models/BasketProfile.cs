using AutoMapper;

namespace MicroStore.ShoppingCart.Api.Models
{
    public class BasketProfile : Profile
    {

        public BasketProfile()
        {
            CreateMap<Basket, BasketDto>()
                .ForMember(x => x.Items, opt => opt.MapFrom(c => c.Items));

            CreateMap<BasketItem, BasketItemDto>();
        }
    }
}
