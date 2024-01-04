using AutoMapper;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
namespace MicroStore.Catalog.Application.Abstractions.Manufacturers.Internals
{
    internal class ManufacturerProfile : Profile
    {
        public ManufacturerProfile()
        {
            CreateMap<Manufacturer, ManufacturerDto>();

            CreateMap<ElasticManufacturer, ManufacturerDto>();
        }
    }
}
