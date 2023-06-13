using AutoMapper;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Domain.Entities;

namespace MicroStore.Catalog.Application.Mappers
{
    public class ProductTagMapper : Profile
    {
        public ProductTagMapper()
        {
            CreateMap<ProductTag, ProductTagDto>();
        }

    }
}
