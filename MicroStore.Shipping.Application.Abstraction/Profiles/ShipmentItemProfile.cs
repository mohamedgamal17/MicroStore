using AutoMapper;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Domain.Entities;

namespace MicroStore.Shipping.Application.Abstraction.Profiles
{
    public class ShipmentItemProfile : Profile
    {
        public ShipmentItemProfile()
        {
            CreateMap<ShipmentItem, ShipmentItemDto>()
                .ForMember(x => x.ItemId, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(x => x.Sku, opt => opt.MapFrom(c => c.Sku))
                .ForMember(x => x.ProductId, opt => opt.MapFrom(c => c.ProductId))
                .ForMember(x => x.Quantity, opt => opt.MapFrom(c => c.Quantity))
                .ForMember(x => x.UnitPrice, opt => opt.MapFrom(c => c.UnitPrice))
                .ForMember(x => x.Dimension, opt => opt.MapFrom(c => c.Dimension))
                .ForMember(x => x.Weight, opt => opt.MapFrom(c => c.Weight));

        }
    }
}
