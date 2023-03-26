using AutoMapper;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Shipments;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Mappers.Shipping
{
    public class ShipmentProfile : Profile
    {
        public ShipmentProfile()
        {
            CreateMap<ShipmentAggregate, ShipmentAggregateVM>()
                .ForMember(x => x.Items, opt => opt.MapFrom(c => c.Items))
                .ForMember(x => x.Address, opt => opt.MapFrom(c => c.Address));

            CreateMap<ShipmentItem, ShipmentItemVM>();

            CreateMap<Shipment, ShipmentVM>()
                .ForMember(x => x.Items, opt => opt.MapFrom(c => c.Items))
                .ForMember(x => x.Address, opt => opt.MapFrom(c => c.Address));

            CreateMap<ShipmentAggregate, ShipmentAggregateVM>()
            .ForMember(x => x.Items, opt => opt.MapFrom(c => c.Items))
            .ForMember(x => x.Address, opt => opt.MapFrom(c => c.Address));

            CreateMap<ShipmentModel, ShipmentCreateRequestOptions>()
                .ForMember(x => x.Address, opt => opt.MapFrom(c => c.Address))
                .ForMember(x => x.Items, opt => opt.MapFrom(c => c.Items));

            CreateMap<ShipmentItemModel, ShipmentItemCreateRequestOptions>();


            CreateMap<ShipmentPackageModel, ShipmentFullfillRequestOptions>()
                .ForMember(x => x.Dimension, opt => opt.MapFrom(c => c.Dimension))
                .ForMember(x => x.Weight, opt => opt.MapFrom(c => c.Weight));



        }
    }
}
