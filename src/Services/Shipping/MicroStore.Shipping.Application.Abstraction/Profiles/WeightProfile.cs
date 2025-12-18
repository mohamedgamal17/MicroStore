using AutoMapper;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Domain.ValueObjects;
namespace MicroStore.Shipping.Application.Abstraction.Profiles
{
    public class WeightProfile : Profile
    {
        public WeightProfile()
        {
            CreateMap<Weight, WeightDto>()
                .ForMember(x => x.Value, opt => opt.MapFrom(c => c.Value))
                .ForMember(x => x.Unit, opt => opt.MapFrom(c=> c.Unit));

        }
    }
}
