using AutoMapper;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;

namespace MicroStore.Catalog.Application.Operations.Manufacturers
{
    public class ElasticManufacturerProfile : Profile
    {
        public ElasticManufacturerProfile()
        {
            CreateMap<Manufacturer, ElasticManufacturer>();
        }
    }
}
