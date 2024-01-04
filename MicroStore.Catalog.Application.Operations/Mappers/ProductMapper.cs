﻿using AutoMapper;
using MicroStore.Catalog.Application.Operations.Etos;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Domain.ValueObjects;
using MicroStore.Catalog.Entities.ElasticSearch;
using MicroStore.Catalog.Entities.ElasticSearch.Common;

namespace MicroStore.Catalog.Application.Operations.Mappers
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Product, ProductEto>()
                .ForMember(x => x.Weight, opt => opt.MapFrom(sr => sr.Weight))
                .ForMember(x => x.Dimensions, opt => opt.MapFrom(sr => sr.Dimensions))
                .ForMember(x => x.Categories, opt => opt.MapFrom(sr => sr.Categories))
                .ForMember(x => x.Manufacturers, opt => opt.MapFrom(sr => sr.Manufacturers))
                .ForMember(x=> x.Tags,opt=> opt.MapFrom(sr=>sr.Tags))
                .ForMember(x => x.ProductImages, opt => opt.MapFrom(c => c.ProductImages.OrderBy(x=> x.DisplayOrder).ToList()))
                .ForMember(x => x.SpecificationAttributes, opt => opt.MapFrom(sr => sr.SpecificationAttributes));

            CreateMap<Weight, WeightEto>()
                .ForMember(x=> x.Unit, opt=> opt.MapFrom(c=> c.Unit.ToString()));

            CreateMap<Dimension, DimensionEto>()
                .ForMember(x=> x.Unit, opt=> opt.MapFrom(c=> c.Unit.ToString()));


            CreateMap<ProductImage, ProductImageEto>()
                .ForMember(x=> x.Image, opt=> opt.MapFrom(sr=> sr.Image));

            CreateMap<Category, ProductCategoryEto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(sr => sr.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(sr => sr.Name))
                .ForMember(x => x.Description, opt => opt.MapFrom(sr => sr.Description));

            CreateMap<Manufacturer, ProductManufacturerEto>()
             .ForMember(x => x.Id, opt => opt.MapFrom(sr => sr.Id))
             .ForMember(x => x.Name, opt => opt.MapFrom(sr => sr.Name))
             .ForMember(x => x.Description, opt => opt.MapFrom(sr => sr.Description));


            CreateMap<ProductEto, ElasticProduct>()
             .ForMember(x => x.Weight, opt => opt.MapFrom(sr => sr.Weight))
             .ForMember(x => x.Dimensions, opt => opt.MapFrom(sr => sr.Dimensions))
             .ForMember(x => x.Categories, opt => opt.MapFrom(sr => sr.Categories))
             .ForMember(x => x.Manufacturers, opt => opt.MapFrom(sr => sr.Manufacturers))
             .ForMember(x => x.Tags, opt => opt.MapFrom(sr => sr.Tags))
             .ForMember(x => x.ProductImages, opt => opt.MapFrom(c => c.ProductImages))
             .ForMember(x => x.SpecificationAttributes, opt => opt.MapFrom(sr => sr.SpecificationAttributes))
             .ForMember(x => x.CreatorId, opt => opt.MapFrom(src => src.CreatorId.ToString()))
             .ForMember(x => x.LastModifierId, opt => opt.MapFrom(src => src.LastModifierId.ToString()))
             .ForMember(x => x.DeletionTime, opt => opt.MapFrom(Src => Src.DeletionTime.ToString()));



            CreateMap<ProductCategoryEto, ElasticProductCategory>()
                .ForMember(x => x.Id, opt => opt.MapFrom(sr => sr.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(sr => sr.Name))
                .ForMember(x => x.Description, opt => opt.MapFrom(sr => sr.Description));


            CreateMap<ProductManufacturerEto, ElasticProductManufacturer>()
             .ForMember(x => x.Id, opt => opt.MapFrom(sr => sr.Id))
             .ForMember(x => x.Name, opt => opt.MapFrom(sr => sr.Name))
             .ForMember(x => x.Description, opt => opt.MapFrom(sr => sr.Description));

            CreateMap<ProductImageEto, ElasticProductImage>();

            CreateMap<WeightEto, ElasticWeight>();

            CreateMap<DimensionEto, ElasticDimension>();

        }
    }
}
