using AutoMapper;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Categories;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Mappers.Catalog
{
    public class CategoryProfile : Profile
    {

        public CategoryProfile()
        {
            CreateMap<Category, CategoryVM>().ReverseMap();

            CreateMap<CategoryModel, CategoryRequestOptions>()
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(x => x.Description, opt => opt.MapFrom(c => c.Description))
                .ReverseMap();

            CreateMap<Category, CategoryModel>().ReverseMap();
        }

    }
}
