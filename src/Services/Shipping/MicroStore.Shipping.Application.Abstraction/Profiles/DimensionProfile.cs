using AutoMapper;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Domain.ValueObjects;
namespace MicroStore.Shipping.Application.Abstraction.Profiles
{
    public class DimensionProfile : Profile
    {
        public DimensionProfile()
        {
            CreateMap<Dimension, DimensionDto>()
                .ForMember(c => c.Height, opt => opt.MapFrom(c => c.Height))
                .ForMember(x => x.Lenght, opt => opt.MapFrom(c => c.Length))
                .ForMember(x => x.Width, opt => opt.MapFrom(c => c.Width))
                .ForMember(x => x.Unit, opt => opt.MapFrom(c => c.Unit));

        }
    }
}
