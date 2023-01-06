using AutoMapper;
using MicroStore.Catalog.Domain.Entities;


namespace MicroStore.Catalog.Application.Abstractions.Categories.Dtos
{
    internal class CategoryProfile : Profile
    {

        public CategoryProfile()
        {

            CreateMap<Category, CategoryDto>()
                .ForMember(x => x.CategoryId, opt => opt.MapFrom(c => c.Id));

            CreateMap<Category, CategoryListDto>()
                .ForMember(x => x.CategoryId, opt => opt.MapFrom(c => c.Id));

        }

    }
}
