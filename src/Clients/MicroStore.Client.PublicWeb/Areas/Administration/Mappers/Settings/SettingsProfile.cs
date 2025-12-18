using AutoMapper;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Settings;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Mappers.Settings
{
    public class SettingsProfile : Profile
    {
        public SettingsProfile()
        {
            CreateMap<ShipmentSettings, ShipmentSettingsModel>()
                .ForMember(x => x.DefaultShippingSystem, opt => opt.MapFrom(c => c.DefaultShippingSystem))
                .ForMember(x => x.Location, opt => opt.MapFrom(c => c.Location));


            CreateMap<ShipmentSettingsModel, ShipmentSettingsRequestOptions>()
                .ForMember(x => x.DefaultShippingSystem, opt => opt.MapFrom(c => c.DefaultShippingSystem))
                .ForMember(x => x.Location, opt => opt.MapFrom(c => c.Location));
        }
    }
}
