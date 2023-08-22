using AutoMapper;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;

namespace MicroStore.Catalog.Application.Operations.Categories
{
    public class ElasticCategoryMapper  : Profile
    {
        public ElasticCategoryMapper()
        {
            CreateMap<Category, ElasticCategory>();
        }
    }
}
