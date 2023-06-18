using AutoMapper;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Domain.Entities;
namespace MicroStore.Catalog.Application.Mappers
{
    public class SpecificationAttributeMapper : Profile
    {
        public SpecificationAttributeMapper()
        {
            CreateMap<SpecificationAttribute, SpecificationAttributeListDto>();

            CreateMap<SpecificationAttribute, SpecificationAttributeDto>()
                .ForMember(x => x.Options, opt => opt.MapFrom(src => src.Options));

            CreateMap<SpecificationAttributeOption, SpecificationAttributeOptionDto>()
                .ForMember(x => x.AttributeId, opt => opt.MapFrom(c => c.SpecificationAttributeId));
        }
    }
}
