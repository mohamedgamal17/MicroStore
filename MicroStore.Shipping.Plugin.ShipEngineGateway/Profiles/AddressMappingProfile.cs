using AutoMapper;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using ShipEngineSDK.Common;
using ShipEngineSDK.Common.Enums;

namespace MicroStore.Shipping.Plugin.ShipEngineGateway.Profiles
{
    public class AddressMappingProfile : Profile
    {
        public AddressMappingProfile()
        {
            CreateMap<MicroStore.Shipping.Domain.ValueObjects.Address, Address>()
                .ForMember(x => x.CountryCode, opt => opt.MapFrom(c => Enum.Parse<Country>(c.CountryCode)))
                .ForMember(x => x.CityLocality, opt => opt.MapFrom(c => c.City))
                .ForMember(x => x.StateProvince, opt => opt.MapFrom(c => c.State))
                .ForMember(x => x.PostalCode, opt => opt.MapFrom(c => c.PostalCode))
                .ForMember(x => x.AddressLine1, opt => opt.MapFrom(c => c.AddressLine1))
                .ForMember(x => x.AddressLine2, opt => opt.MapFrom(c => c.AddressLine2))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(x => x.Phone, opt => opt.MapFrom(c => c.Phone));


            CreateMap<Address,AddressDto>()
                .ForMember(x => x.CountryCode, opt => opt.MapFrom(c => c.ToString()))
                .ForMember(x => x.City, opt => opt.MapFrom(c => c.CityLocality))
                .ForMember(x => x.State, opt => opt.MapFrom(c => c.StateProvince))
                .ForMember(x => x.PostalCode, opt => opt.MapFrom(c => c.PostalCode))
                .ForMember(x => x.AddressLine1, opt => opt.MapFrom(c => c.AddressLine1))
                .ForMember(x => x.AddressLine2, opt => opt.MapFrom(c => c.AddressLine2))
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(x => x.Phone, opt => opt.MapFrom(c => c.Phone));
        }
    }
}
