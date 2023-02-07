using AutoMapper;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using MicroStore.Ordering.Application.StateMachines;

namespace MicroStore.Ordering.Application.Mappers
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


            CreateMap<OrderItemModel, OrderItemEntity>();

            CreateMap<OrderItemModel, IntegrationEvents.Models.OrderItemModel>().ReverseMap();

        }
    }
}
