using AutoMapper;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
namespace MicroStore.Catalog.Application.Abstractions.Categories.Internals
{
    internal class CategoryProfile : Profile
    {
        public CategoryProfile()
        {

            CreateMap<Category, CategoryDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id));

            CreateMap<ElasticCategory, CategoryDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(x => x.Description, opt => opt.MapFrom(c => c.Description))
                .ForMember(x => x.CreationTime, opt => opt.MapFrom(c => c.CreationTime));
        }

    }
}
