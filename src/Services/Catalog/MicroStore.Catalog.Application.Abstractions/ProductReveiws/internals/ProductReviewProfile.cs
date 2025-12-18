using AutoMapper;
using MicroStore.Catalog.Domain.Entities;

namespace MicroStore.Catalog.Application.Abstractions.ProductReveiws.internals
{
    internal class ProductReviewProfile : Profile
    {
        public ProductReviewProfile()
        {
            CreateMap<ProductReview, ProductReviewDto>();
        }
    }
}
