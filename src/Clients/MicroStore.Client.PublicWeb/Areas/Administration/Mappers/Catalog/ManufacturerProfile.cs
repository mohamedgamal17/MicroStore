using AutoMapper;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Manufacturers;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Mappers.Catalog
{
    public class ManufacturerProfile : Profile
    {
        public ManufacturerProfile()
        {
            CreateMap<Manufacturer, ManufacturerVM>();
            CreateMap<Manufacturer, ManufacturerModel>();
            CreateMap<ManufacturerModel , ManufacturerRequestOptions>();
        }
    }
}
