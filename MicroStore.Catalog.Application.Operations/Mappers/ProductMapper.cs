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
                .ForMember(x => x.ProductCategories, opt => opt.MapFrom(sr => sr.ProductCategories))
                .ForMember(x => x.ProductManufacturers, opt => opt.MapFrom(sr => sr.ProductManufacturers))
                .ForMember(x => x.ProductImages, opt => opt.MapFrom(c => c.ProductImages))
                .ForMember(x => x.SpecificationAttributes, opt => opt.MapFrom(sr => sr.SpecificationAttributes));

            CreateMap<Weight, WeightEto>()
                .ForMember(x=> x.Unit, opt=> opt.MapFrom(c=> c.Unit.ToString()));

            CreateMap<Dimension, DimensionEto>()
                .ForMember(x=> x.Unit, opt=> opt.MapFrom(c=> c.Unit.ToString()));


            CreateMap<ProductImage, ProductImageEto>()
                .ForMember(x=> x.Image, opt=> opt.MapFrom(sr=> sr.ImagePath));



            CreateMap<ProductEto, ElasticProduct>()
             .ForMember(x => x.Weight, opt => opt.MapFrom(sr => sr.Weight))
             .ForMember(x => x.Dimensions, opt => opt.MapFrom(sr => sr.Dimensions))
             .ForMember(x => x.ProductCategories, opt => opt.MapFrom(sr => sr.ProductCategories))
             .ForMember(x => x.ProductManufacturers, opt => opt.MapFrom(sr => sr.ProductManufacturers))
             .ForMember(x => x.ProductImages, opt => opt.MapFrom(c => c.ProductImages))
             .ForMember(x => x.SpecificationAttributes, opt => opt.MapFrom(sr => sr.SpecificationAttributes));

            CreateMap<ProductImageEto, ElasticProductImage>();

            CreateMap<WeightEto, ElasticWeight>();

            CreateMap<DimensionEto, ElasticDimension>();

        }
    }
}