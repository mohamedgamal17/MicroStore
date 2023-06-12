using AutoMapper;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models.Products;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Domain.ValueObjects;
namespace MicroStore.Catalog.Application.Mappers
{
    internal class ProductMapper : Profile
    {

        public ProductMapper()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.Sku, opt => opt.MapFrom(c => c.Sku))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(x => x.ShortDescription, opt => opt.MapFrom(c => c.ShortDescription))
                .ForMember(x => x.LongDescription, opt => opt.MapFrom(c => c.LongDescription))
                .ForMember(x => x.Price, opt => opt.MapFrom(c => c.Price))
                .ForMember(x => x.OldPrice, opt => opt.MapFrom(c => c.OldPrice))
                .ForMember(x => x.Weight, opt => opt.MapFrom(c => c.Weight))
                .ForMember(x => x.Dimensions, opt => opt.MapFrom(c => c.Dimensions))
                .ForMember(x => x.ProductCategories, opt => opt.MapFrom(c => c.ProductCategories))
                .ForMember(x => x.ProductManufacturers, opt => opt.MapFrom(c => c.ProductManufacturers))
                .ForMember(x => x.ProductTags, opt => opt.MapFrom(c => c.ProductTags));



            CreateMap<ProductCategory, ProductCategoryDto>()
                .ForMember(x => x.CategoryId, opt => opt.MapFrom(x => x.CategoryId))
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Category.Name));


            CreateMap<ProductImage, ProductImageDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.DisplayOrder, opt => opt.MapFrom(c => c.DisplayOrder))
                .ForMember(x => x.Image, opt => opt.MapFrom(c => c.ImagePath));


            CreateMap<ProductManufacturer, ProductManufacturerDto>()
                .ForMember(x => x.Manufacturer, opt => opt.MapFrom(c => c.Manufacturer));


            CreateMap<Dimension, DimensionModel>()
                .ForMember(x => x.Unit, opt => opt.MapFrom(c => ConvertDimensionUnit(c.Unit)));

            CreateMap<Weight, WeightModel>()
                .ForMember(x => x.Unit, opt => opt.MapFrom(c => ConvertWeightUnit(c.Unit)));
        }



        public static string ConvertWeightUnit(WeightUnit unit)
        {
            switch (unit)
            {
                case WeightUnit.Gram:
                    return "g";
                case WeightUnit.KiloGram:
                    return "kg";
                case WeightUnit.Pound:
                    return "lb";
                default:
                    return "none";
            }
        }

        public static string ConvertDimensionUnit(DimensionUnit unit)
        {
            switch (unit)
            {
                case DimensionUnit.CentiMeter:
                    return "cm";
                case DimensionUnit.Inch:
                    return "inch";
                default:
                    return "none";
            }
        }
    }
}
