using AutoMapper;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;

namespace MicroStore.Catalog.Application.Operations.ProductTags
{
    public class ProductTagProfile : Profile
    {
        public ProductTagProfile()
        {
            CreateMap<Tag, ElasticTag>();
        }
    }
}
