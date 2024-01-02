using AutoMapper;
using MicroStore.Catalog.Domain.Entities;
namespace MicroStore.Catalog.Application.Abstractions.Manufacturers.Internals
{
    internal class ManufacturerProfile : Profile
    {
        public ManufacturerProfile()
        {
            CreateMap<Manufacturer, ManufacturerDto>();
        }
    }
}
