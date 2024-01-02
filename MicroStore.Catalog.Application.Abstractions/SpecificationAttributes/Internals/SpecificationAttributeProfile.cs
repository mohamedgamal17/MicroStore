using AutoMapper;
using MicroStore.Catalog.Domain.Entities;
namespace MicroStore.Catalog.Application.Abstractions.SpecificationAttributes.Internals
{
    public class SpecificationAttributeProfile : Profile
    {
        public SpecificationAttributeProfile()
        {
            CreateMap<SpecificationAttribute, SpecificationAttributeListDto>();

            CreateMap<SpecificationAttribute, SpecificationAttributeDto>()
                .ForMember(x => x.Options, opt => opt.MapFrom(src => src.Options));

            CreateMap<SpecificationAttributeOption, SpecificationAttributeOptionDto>()
                .ForMember(x => x.AttributeId, opt => opt.MapFrom(c => c.SpecificationAttributeId));
        }
    }
}
