using AutoMapper;
using MicroStore.Catalog.Application.Operations.Etos;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;

namespace MicroStore.Catalog.Application.Operations.Mappers
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<Category, CategoryEto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(sr => sr.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(sr => sr.Name))
                .ForMember(x => x.Description, opt => opt.MapFrom(sr => sr.Description));

            CreateMap<CategoryEto, ElasticCategory>()
                .ForMember(x => x.CreatorId, opt => opt.MapFrom(src => src.CreatorId.ToString()))
                .ForMember(x => x.LastModifierId, opt => opt.MapFrom(src => src.LastModifierId.ToString()))
                .ForMember(x => x.DeletionTime, opt => opt.MapFrom(Src => Src.DeletionTime.ToString()));

        }
    }
}
