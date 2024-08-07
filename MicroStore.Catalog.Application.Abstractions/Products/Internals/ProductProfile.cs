﻿using AutoMapper;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Domain.ValueObjects;
using MicroStore.Catalog.Entities.ElasticSearch;
using MicroStore.Catalog.Entities.ElasticSearch.Common;
namespace MicroStore.Catalog.Application.Abstractions.Products.Internals
{
    internal class ProductProfile : Profile
    {
        public ProductProfile()
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
                .ForMember(x => x.Categories, opt => opt.MapFrom(c => c.Categories))
                .ForMember(x => x.Manufacturers, opt => opt.MapFrom(c => c.Manufacturers))
                .ForMember(x => x.Tags, opt => opt.MapFrom(c => c.Tags))
                .ForMember(x => x.Images, opt => opt.MapFrom(c => c.ProductImages));


            CreateMap<ElasticProduct, ProductDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(x => x.Sku, opt => opt.MapFrom(c => c.Sku))
                .ForMember(x => x.ShortDescription, opt => opt.MapFrom(c => c.ShortDescription))
                .ForMember(x => x.LongDescription, opt => opt.MapFrom(c => c.LongDescription))
                .ForMember(x => x.Price, opt => opt.MapFrom(c => c.Price))
                .ForMember(x => x.OldPrice, opt => opt.MapFrom(c => c.OldPrice))
                .ForMember(x => x.Weight, opt => opt.MapFrom(c => c.Weight))
                .ForMember(x => x.Dimensions, opt => opt.MapFrom(c => c.Dimensions))
                .ForMember(x => x.Categories, opt => opt.MapFrom(c => c.Categories))
                .ForMember(x => x.Manufacturers, opt => opt.MapFrom(c => c.Manufacturers))
                .ForMember(x => x.Tags, opt => opt.MapFrom(c => c.Tags))
                .ForMember(x => x.Images, opt => opt.MapFrom(c => c.ProductImages));

            CreateMap<Category, ProductCategoryDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.Description));

            CreateMap<ElasticProductCategory, ProductCategoryDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(x => x.Description, opt => opt.MapFrom(c => c.Description));

            CreateMap<ProductImage, ProductImageDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.DisplayOrder, opt => opt.MapFrom(c => c.DisplayOrder))
                .ForMember(x => x.Image, opt => opt.MapFrom(c => c.Image));


            CreateMap<ElasticProductImage, ProductImageDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.DisplayOrder, opt => opt.MapFrom(c => c.DisplayOrder))
                .ForMember(x => x.Image, opt => opt.MapFrom(c => c.Image));


            CreateMap<Manufacturer, ProductManufacturerDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(x => x.Description, opt => opt.MapFrom(c => c.Description));

            CreateMap<ElasticProductManufacturer, ProductManufacturerDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(x => x.Description, opt => opt.MapFrom(c => c.Description));

            CreateMap<ProductSpecificationAttribute, ProductSpecificationAttributeDto>()
                .ForMember(x => x.Attribute, opt => opt.MapFrom(c => c.Attribute))
                .ForMember(x => x.Option, opt => opt.MapFrom(c => c.Option));


            CreateMap<Dimension, DimensionModel>()
                .ForMember(x => x.Unit, opt => opt.MapFrom(c => c.Unit.ToString()));

            CreateMap<Weight, WeightModel>()
                .ForMember(x => x.Unit, opt => opt.MapFrom(c => c.Unit.ToString()));

            CreateMap<ElasticWeight, WeightModel>();
            CreateMap<ElasticDimension, DimensionModel>();

        }
    }
}
