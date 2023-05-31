using AutoMapper;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.StateMachines;
using MicroStore.Ordering.IntegrationEvents;

namespace MicroStore.Ordering.Application.Mappers
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderStateEntity, OrderListDto>()
                .ForMember(x => x.UserId, opt => opt.MapFrom(c => c.UserId))
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.CorrelationId))
                .ForMember(x => x.BillingAddress, opt => opt.MapFrom(c => c.BillingAddress))
                .ForMember(x => x.ShippingAddress, opt => opt.MapFrom(c => c.ShippingAddress))
                .ForMember(x=> x.Total, opt=> opt.MapFrom(c=> c.TotalPrice))
                .ForMember(x => x.CurrentState, opt => opt.MapFrom(c => c.CurrentState));

            CreateMap<OrderStateEntity, OrderDto>()
                .ForMember(x => x.UserId, opt => opt.MapFrom(c => c.UserId))
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.CorrelationId))
                .ForMember(x => x.BillingAddress, opt => opt.MapFrom(c => c.BillingAddress))
                .ForMember(x => x.ShippingAddress, opt => opt.MapFrom(c => c.ShippingAddress))
                .ForMember(x => x.CurrentState, opt => opt.MapFrom(c => c.CurrentState))
                .ForMember(x => x.Total, opt => opt.MapFrom(c => c.TotalPrice))
                .ForMember(x => x.Items, opt => opt.MapFrom(c => c.OrderItems));


            CreateMap<OrderSubmitedEvent, OrderStateEntity>()
                .ForMember(x => x.OrderItems, opt => opt.MapFrom(c => c.OrderItems))
                .ForMember(x => x.BillingAddress, opt => opt.MapFrom(c => c.BillingAddress))
                .ForMember(x => x.ShippingAddress, opt => opt.MapFrom(c => c.ShippingAddress));


            CreateMap<SubmitOrderIntegrationEvent, OrderSubmitedEvent>()
                .ForMember(x => x.BillingAddress, opt => opt.MapFrom(c => c.BillingAddress))
                .ForMember(x => x.ShippingAddress, opt => opt.MapFrom(c => c.ShippingAddress))
                .ForMember(x => x.OrderItems, opt => opt.MapFrom(c => c.OrderItems));
        }
    }
}
