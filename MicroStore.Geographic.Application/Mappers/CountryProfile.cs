using AutoMapper;
using MicroStore.Geographic.Application.Domain;
using MicroStore.Geographic.Application.Dtos;
using MicroStore.Geographic.Application.Models;

namespace MicroStore.Geographic.Application.Mappers
{
    public class CountryProfile  :Profile
    {
        public CountryProfile()
        {
            CreateMap<CountryModel, Country>();

            CreateMap<Country, CountryListDto>();

            CreateMap<Country, CountryDto>()
                .ForMember(x => x.StateProvinces, opt => opt.MapFrom(c => c.StateProvinces));
        }
    }
}
