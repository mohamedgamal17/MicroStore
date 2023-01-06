using AutoMapper;
using MicroStore.Inventory.Application.Abstractions.Dtos;
using MicroStore.Inventory.Domain.OrderAggregate;
using MicroStore.Inventory.Domain.ValueObjects;
namespace MicroStore.Inventory.Application.Abstractions.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(x => x.OrderId, opt => opt.MapFrom(c => c.Id))
                .ForMember(x=> x.ShippingAddress, opt=> opt.MapFrom(c=> c.ShippingAddress))
                .ForMember(x=> x.BillingAddres , opt=> opt.MapFrom(c=> c.BillingAddres))
                .ForMember(x => x.Items, opt => opt.MapFrom(c => c.Items));

            CreateMap<Order, OrderListDto>()
               .ForMember(x => x.OrderId, opt => opt.MapFrom(c => c.Id))
               .ForMember(x => x.ShippingAddress, opt => opt.MapFrom(c => c.ShippingAddress))
               .ForMember(x => x.BillingAddres, opt => opt.MapFrom(c => c.BillingAddres));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(x => x.ItemId, opt => opt.MapFrom(c => c.Id));

            CreateMap<Address, AddressDto>();             
        }
    }
}
