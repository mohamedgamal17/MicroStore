using AutoMapper;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;

namespace MicroStore.Catalog.Application.Operations.ProductReviews
{
    public class ElasticProductReviewProfile : Profile
    {
        public ElasticProductReviewProfile()
        {
            CreateMap<ProductReview, ElasticProductReview>();
        }
    }
}
