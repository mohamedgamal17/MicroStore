using AutoMapper;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Domain.Entities;
namespace MicroStore.Shipping.Application.Abstraction.Profiles
{
    public class ShipmentProfile : Profile
    {
        public ShipmentProfile()
        {
            CreateMap<Shipment, ShipmentDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.OrderId, opt => opt.MapFrom(c => c.OrderId))
                .ForMember(x => x.ShipmentExternalId, opt => opt.MapFrom(c => c.ShipmentExternalId))
                .ForMember(x => x.ShipmentLabelExternalId, opt => opt.MapFrom(c => c.ShipmentLabelExternalId))
                .ForMember(x=> x.TrackingNumber, opt=> opt.MapFrom(c=> c.TrackingNumber))
                .ForMember(x => x.UserId, opt => opt.MapFrom(c => c.UserId))
                .ForMember(x => x.Address, opt => opt.MapFrom(c => c.Address))
                .ForMember(x => x.Items, opt => opt.MapFrom(c => c.Items))
                .ForMember(x => x.Status, opt => opt.MapFrom(c => c.Status));

            CreateMap<Shipment , ShipmentListDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.OrderId, opt => opt.MapFrom(c => c.OrderId))
                .ForMember(x => x.ShipmentExternalId, opt => opt.MapFrom(c => c.ShipmentExternalId))
                .ForMember(x => x.ShipmentLabelExternalId, opt => opt.MapFrom(c => c.ShipmentLabelExternalId))
                .ForMember(x => x.TrackingNumber, opt => opt.MapFrom(c => c.TrackingNumber))
                .ForMember(x => x.UserId, opt => opt.MapFrom(c => c.UserId))
                .ForMember(x => x.Address, opt => opt.MapFrom(c => c.Address))
                .ForMember(x => x.Status, opt => opt.MapFrom(c => c.Status));

        }
    }
}
