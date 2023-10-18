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

            CreateMap<ProductReviewEto , ElasticProductReview>()
                .ForMember(x => x.CreatorId, opt => opt.MapFrom(src => src.CreatorId.ToString()))
                .ForMember(x => x.LastModifierId, opt => opt.MapFrom(src => src.LastModifierId.ToString()))
                .ForMember(x => x.DeletionTime, opt => opt.MapFrom(Src => Src.DeletionTime.ToString())); 

        }
    }
}
