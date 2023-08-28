using AutoMapper;
using MicroStore.Catalog.Application.Operations.Etos;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;

namespace MicroStore.Catalog.Application.Operations.Mappers
{
    public class ProductReviewMapper : Profile
    {
        public ProductReviewMapper()
        {
            CreateMap<ProductReview, ProductReviewEto>();

            CreateMap<ProductReviewEto , ElasticProductReview >();
               
        }
    }
}
