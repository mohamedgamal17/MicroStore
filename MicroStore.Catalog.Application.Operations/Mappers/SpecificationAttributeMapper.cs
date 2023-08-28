using AutoMapper;
using MicroStore.Catalog.Application.Operations.Etos;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
namespace MicroStore.Catalog.Application.Operations.Mappers
{
    public class SpecificationAttributeMapper : Profile
    {
        public SpecificationAttributeMapper()
        {
            CreateMap<SpecificationAttribute, SpecificationAttributeEto>()
                .ForMember(x => x.Name, opt => opt.MapFrom(sr => sr.Name));

            CreateMap<SpecificationAttributeOption, SpecificationAttributeOptionEto>()
                .ForMember(x => x.Value, opt => opt.MapFrom(sr => sr.Name));

            CreateMap<SpecificationAttributeEto , ElasticSpecificationAttribute>()
                .ForMember(x => x.Name, opt => opt.MapFrom(sr => sr.Name))
                .ForMember(x => x.Options, opt => opt.MapFrom(sr => sr.Options))
                .ReverseMap();


            CreateMap<SpecificationAttributeOptionEto , ElasticSpecificationAttributeOption>()
                .ForMember(x => x.Value, opt => opt.MapFrom(sr => sr.Value))
                 .ReverseMap();

            CreateMap<ProductSpecificationAttribute, ProductSpecificationAttributeEto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(sr => sr.Id))
                .ForMember(x => x.AttributeId, opt => opt.MapFrom(sr => sr.AttributeId))
                .ForMember(x => x.OptionId, opt => opt.MapFrom(sr => sr.OptionId))
                .ForMember(x => x.Name, opt => opt.MapFrom(sr => sr.Attribute.Name))
                .ForMember(x => x.Value, opt => opt.MapFrom(sr => sr.Option.Name));

            CreateMap<ProductSpecificationAttributeEto, ElasticProductSpecificationAttribute>();
        }
    }
}
