using AutoMapper;
using MicroStore.Ordering.Application.Abstractions.Dtos;
using MicroStore.Ordering.Application.Abstractions.StateMachines;
namespace MicroStore.Ordering.Application.Abstractions.Profiles
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
                .ForMember(x => x.CurrentState, opt => opt.MapFrom(c => c.CurrentState));

            CreateMap<OrderStateEntity, OrderDto>()
                .ForMember(x => x.UserId, opt => opt.MapFrom(c => c.UserId))
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.CorrelationId))
                .ForMember(x => x.BillingAddress, opt => opt.MapFrom(c => c.BillingAddress))
                .ForMember(x => x.ShippingAddress, opt => opt.MapFrom(c => c.ShippingAddress))
                .ForMember(x => x.CurrentState, opt => opt.MapFrom(c => c.CurrentState))
                .ForMember(x => x.OrderItems, opt => opt.MapFrom(c => c.OrderItems));
        }
    }
}
