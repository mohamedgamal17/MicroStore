using AutoMapper;
using MicroStore.Catalog.Domain.Entities;

namespace MicroStore.Catalog.Application.Abstractions.ProductTags.Internals
{
    internal class ProductTagProfile : Profile
    {
        public ProductTagProfile()
        {
            CreateMap<Tag, ProductTagDto>();
        }

    }
}
