using AutoMapper;
using MicroStore.ShoppingCart.Application.Abstraction.Dtos;
using MicroStore.ShoppingCart.Domain.Entities;

namespace MicroStore.ShoppingCart.Application.Abstraction
{
    internal class BasketProfile : Profile
    {

        public BasketProfile()
        {
            CreateMap<Basket, BasketDto>()
                .ForMember(x => x.BasketId, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.UserId, opt => opt.MapFrom(c => c.UserId))
                .ForMember(x => x.LineItems, opt => opt.MapFrom(c => c.LineItems));

            CreateMap<BasketItem, BasketItemDto>()
                .ForMember(x => x.ItemId, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.Product, opt => opt.MapFrom(c => c.Product))
                .ForMember(x => x.Quantity, opt => opt.MapFrom(c => c.Quantity));


            CreateMap<Product, ProductDTO>()
                .ForMember(x => x.ProductId, opt => opt.MapFrom(c => c.Id));
        }
    }
}
