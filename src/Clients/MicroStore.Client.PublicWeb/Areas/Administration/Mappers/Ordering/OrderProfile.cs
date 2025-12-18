using AutoMapper;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Products;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Ordering;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Mappers.Ordering
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderAggregate, OrderAggregateVM>()
                .ForMember(x => x.Items, opt => opt.MapFrom(c => c.Items))
                .ForMember(x=> x.User, opt=> opt.MapFrom(c=> c.User))
                .ForMember(x => x.Payment, opt => opt.MapFrom(c => c.Payment))
                .ForMember(x => x.Shipment, opt => opt.MapFrom(c => c.Shipment))
                .ForMember(x => x.ShippingAddress, opt => opt.MapFrom(c => c.ShippingAddress))
                .ForMember(x => x.BillingAddress, opt => opt.MapFrom(c => c.BillingAddress));

            CreateMap<Order, OrderVM>()
                .ForMember(x => x.BillingAddress, opt => opt.MapFrom(c => c.BillingAddress))
                .ForMember(x => x.ShippingAddress, opt => opt.MapFrom(c => c.ShippingAddress))
                .ForMember(x => x.Items, opt => opt.MapFrom(c => c.Items));

            CreateMap<OrderItem, OrderItemVM>();
                

            CreateMap<PagedList<Order>, PagedList<OrderVM>>();
        }

    }
}
