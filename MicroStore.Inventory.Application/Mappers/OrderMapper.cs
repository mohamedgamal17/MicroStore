using AutoMapper;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Domain.OrderAggregate;
using MicroStore.Inventory.Domain.ValueObjects;
namespace MicroStore.Inventory.Application.Mappers
{
    public class OrderMapper : Profile
    {
        public OrderMapper()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.ShippingAddress, opt => opt.MapFrom(c => c.ShippingAddress))
                .ForMember(x => x.BillingAddres, opt => opt.MapFrom(c => c.BillingAddres))
                .ForMember(x => x.Items, opt => opt.MapFrom(c => c.Items));

            CreateMap<Order, OrderListDto>()
               .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
               .ForMember(x => x.ShippingAddress, opt => opt.MapFrom(c => c.ShippingAddress))
               .ForMember(x => x.BillingAddres, opt => opt.MapFrom(c => c.BillingAddres));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id));

            CreateMap<Address, AddressDto>();
        }
    }
}
