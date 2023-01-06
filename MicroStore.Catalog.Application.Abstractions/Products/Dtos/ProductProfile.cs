using AutoMapper;
using MicroStore.Catalog.Application.Abstractions.Common.Models;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Domain.ValueObjects;
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
                .ForMember(x => x.Weight, opt => opt.MapFrom(c => c.Weight))
                .ForMember(x => x.Dimensions, opt => opt.MapFrom(c => c.Dimensions))
                .ForMember(x => x.ProductCategories, opt => opt.MapFrom(c => c.ProductCategories));


            CreateMap<Product,ProductListDto>()
                .ForMember(x => x.ProductId, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.Sku, opt => opt.MapFrom(c => c.Sku))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(x => x.ShortDescription, opt => opt.MapFrom(c => c.ShortDescription))
                .ForMember(x => x.LongDescription, opt => opt.MapFrom(c => c.LongDescription))
                .ForMember(x => x.Price, opt => opt.MapFrom(c => c.Price))
                .ForMember(x => x.OldPrice, opt => opt.MapFrom(c => c.OldPrice))
                .ForMember(x=> x.Weight, opt=>opt.MapFrom(c=> c.Weight))
                .ForMember(x=> x.Dimensions, opt=>opt.MapFrom(c=> c.Dimensions))
                .ForMember(x => x.ProductCategories, opt => opt.MapFrom(c => c.ProductCategories));

            CreateMap<ProductCategory, ProductCategoryDto>()
                .ForMember(x => x.CateogryId, opt => opt.MapFrom(x => x.CategoryId))
                .ForMember(x => x.IsFeaturedProduct, opt => opt.MapFrom(x => x.IsFeaturedProduct))
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Category.Name));


            CreateMap<ProductImage, ProductImageDto>()
                .ForMember(x => x.ProductImageId, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.DisplayOrder, opt => opt.MapFrom(c => c.DisplayOrder))
                .ForMember(x => x.Image, opt => opt.MapFrom(c => c.ImagePath));


            CreateMap<Dimension, DimensionModel>()
                .ForMember(x => x.Unit, opt => opt.MapFrom(c => c.Unit.ToString()));

            CreateMap<Weight, WeightModel>()
                .ForMember(x => x.Unit, opt => opt.MapFrom(c => c.Unit.ToString()));
        }
    }
}
