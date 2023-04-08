using AutoMapper;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Domain.Entities;

namespace MicroStore.Catalog.Application.Mappers
{
    internal class ManufacturerMapper : Profile
    {
        public ManufacturerMapper()
        {
            CreateMap<Manufacturer, ManufacturerDto>();
        }
    }
}
