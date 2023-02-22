using AutoMapper;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Domain.Entities;

namespace MicroStore.Shipping.Application.Abstraction.Profiles
{
    public class ShipmentSystemProfile :Profile
    {
        public ShipmentSystemProfile()
        {
            CreateMap<ShippingSystem, ShippingSystemDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id));
        }
    }
}
