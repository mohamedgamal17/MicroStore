using AutoMapper;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;

namespace MicroStore.Catalog.Application.Abstractions.ProductTags.Internals
{
    internal class ProductTagProfile : Profile
    {
        public ProductTagProfile()
        {
            CreateMap<Tag, ProductTagDto>();

            CreateMap<ElasticTag, ProductTagDto>();

        }

    }
}
