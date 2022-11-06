using AutoMapper;
using MicroStore.Catalog.Domain.Entities;


namespace MicroStore.Catalog.Application.Abstractions.Products.Dtos
{
    internal class ProductProfile : Profile
    {

        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(x => x.ProductId, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.Sku, opt => opt.MapFrom(c => c.Sku))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(x => x.ShortDescription, opt => opt.MapFrom(c => c.ShortDescription))
                .ForMember(x => x.LongDescription, opt => opt.MapFrom(c => c.LongDescription))
                .ForMember(x => x.Price, opt => opt.MapFrom(c => c.Price))
                .ForMember(x => x.OldPrice, opt => opt.MapFrom(c => c.OldPrice))
                .ForMember(x => x.ProductCategories, opt => opt.MapFrom(c => c.ProductCategories));




            CreateMap<ProductCategory, ProductCategoryDto>()
                .ForMember(x => x.ProductCategoryId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.IsFeaturedProduct, opt => opt.MapFrom(x => x.IsFeaturedProduct))
                .ForMember(x => x.Category, opt => opt.MapFrom(x => x.Category));
        }
    }
}
