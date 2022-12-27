using AutoMapper;
using ShipEngineSDK.Common;

namespace MicroStore.Shipping.Plugin.ShipEngineGateway.Profiles
{
    public class ShipmentItemMappingProfile : Profile
    {
        public ShipmentItemMappingProfile()
        {
            CreateMap<MicroStore.Shipping.Domain.Entities.ShipmentItem, ShipmentItem>()
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(x => x.Quantity, opt => opt.MapFrom(c => c.Quantity))
                .ForMember(x => x.Sku, opt => opt.MapFrom(c => c.Sku))
                .ReverseMap();
            
        }
    }
}
