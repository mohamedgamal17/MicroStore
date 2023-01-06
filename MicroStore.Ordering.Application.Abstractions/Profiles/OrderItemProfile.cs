using AutoMapper;
using MicroStore.Ordering.Application.Abstractions.Dtos;
using MicroStore.Ordering.Application.Abstractions.StateMachines;

namespace MicroStore.Ordering.Application.Abstractions.Profiles
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<OrderItemEntity, OrderItemDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.ExternalProductId, opt => opt.MapFrom(c => c.ExternalProductId))
                .ForMember(x => x.Sku, opt => opt.MapFrom(c => c.Sku))
                .ForMember(x => x.Quantity, opt => opt.MapFrom(c => c.Quantity))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(x => x.UnitPrice, opt => opt.MapFrom(c => c.UnitPrice));
        }
    }
}
