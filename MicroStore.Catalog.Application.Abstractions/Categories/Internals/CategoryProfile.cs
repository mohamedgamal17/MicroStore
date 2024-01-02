using AutoMapper;
using MicroStore.Catalog.Domain.Entities;
namespace MicroStore.Catalog.Application.Abstractions.Categories.Internals
{
    internal class CategoryProfile : Profile
    {
        public CategoryProfile()
        {

            CreateMap<Category, CategoryDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id));
        }

    }
}
