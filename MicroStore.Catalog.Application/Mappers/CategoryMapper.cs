using AutoMapper;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Domain.Entities;


namespace MicroStore.Catalog.Application.Mappers
{
    internal class CategoryMapper : Profile
    {

        public CategoryMapper()
        {

            CreateMap<Category, CategoryDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id));

            CreateMap<Category, CategoryListDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id));

        }

    }
}
