using AutoMapper;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Domain.ValueObjects;
using MicroStore.Catalog.Entities.ElasticSearch;
using MicroStore.Catalog.Entities.ElasticSearch.Common;

namespace MicroStore.Catalog.Application.Operations.Products
{
    public class ElasticProductMapper : Profile
    {
        public ElasticProductMapper()
        {
            CreateMap<Product, ElasticProduct>()
                .ForMember(x => x.ProductCategories, opt => opt.MapFrom(c => c.ProductCategories))
                .ForMember(x => x.ProductManufacturers, opt => opt.MapFrom(c => c.ProductManufacturers))
                .ForMember(x => x.ProductTags, opt => opt.MapFrom(c => c.ProductTags))
                .ForMember(x => x.Dimensions, opt => opt.MapFrom(c => c.Dimensions))
                .ForMember(x => x.Weight, opt => opt.MapFrom(c => c.Weight))
                .ForMember(x => x.ProductImages, opt => opt.MapFrom(c => c.ProductImages))
                .ForMember(x => x.SpecificationAttributes, opt => opt.MapFrom(c => c.SpecificationAttributes));


            CreateMap<ProductImage, ElasticProductImage>();

            CreateMap<ProductCategory, ElasticCategory>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Category.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Category.Name))
                .ForMember(x => x.Description, opt => opt.MapFrom(c => c.Category.Description));

            CreateMap<ProductCategory, ElasticCategory>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Category.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Category.Name))
                .ForMember(x => x.Description, opt => opt.MapFrom(c => c.Category.Description));

            CreateMap<Dimension, ElasticDimension>()
                .ForMember(x => x.Height, opt => opt.MapFrom(c => c.Height))
                .ForMember(x => x.Length, opt => opt.MapFrom(c => c.Length))
                .ForMember(x => x.Width, opt => opt.MapFrom(c => c.Width))
                .ForMember(x => x.Unit, opt => opt.MapFrom(c => c.Unit.ToString()));

            CreateMap<Weight, ElasticWeight>()
                .ForMember(x => x.Value, opt => opt.MapFrom(c => c.Value))
                .ForMember(x => x.Unit, opt => opt.MapFrom(c => c.Unit));

            CreateMap<ProductManufacturer, ElasticManufacturer>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Manufacturer.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Manufacturer.Name))
                .ForMember(x => x.Description, opt => opt.MapFrom(c => c.Manufacturer.Description));

            CreateMap<ProductSpecificationAttribute, ElasticProductSpecificationAttribute>()
                .ForMember(x => x.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Attribute.Name))
                .ForMember(x => x.Value, opt => opt.MapFrom(c => c.Option.Name))
                .ForMember(x => x.AttributeId, opt => opt.MapFrom(c => c.Attribute))
                .ForMember(x => x.OptionId, opt => opt.MapFrom(c => c.OptionId));






        }
    }
}
