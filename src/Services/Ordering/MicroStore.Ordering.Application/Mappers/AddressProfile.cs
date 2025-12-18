using AutoMapper;
using MicroStore.Ordering.Application.Domain;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
namespace MicroStore.Ordering.Application.Mappers
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressDto>()
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(x => x.Phone, opt => opt.MapFrom(c => c.Phone))
                .ForMember(x => x.CountryCode, opt => opt.MapFrom(c => c.CountryCode))
                .ForMember(x => x.State, opt => opt.MapFrom(c => c.State))
                .ForMember(x => x.City, opt => opt.MapFrom(c => c.City))
                .ForMember(x => x.AddressLine1, opt => opt.MapFrom(c => c.AddressLine1))
                .ForMember(x => x.AddressLine2, opt => opt.MapFrom(c => c.AddressLine2))
                .ForMember(x => x.Zip, opt => opt.MapFrom(c => c.Zip))
                .ForMember(x => x.PostalCode, opt => opt.MapFrom(c => c.PostalCode));




            CreateMap<AddressModel, Address>()
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(x => x.Phone, opt => opt.MapFrom(c => c.Phone))
                .ForMember(x => x.CountryCode, opt => opt.MapFrom(c => c.CountryCode))
                .ForMember(x => x.State, opt => opt.MapFrom(c => c.State))
                .ForMember(x => x.City, opt => opt.MapFrom(c => c.City))
                .ForMember(x => x.AddressLine1, opt => opt.MapFrom(c => c.AddressLine1))
                .ForMember(x => x.AddressLine2, opt => opt.MapFrom(c => c.AddressLine2));




            CreateMap<IntegrationEvents.Models.AddressModel, AddressModel>()
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(x => x.Phone, opt => opt.MapFrom(c => c.Phone))
                .ForMember(x => x.CountryCode, opt => opt.MapFrom(c => c.CountryCode))
                .ForMember(x => x.State, opt => opt.MapFrom(c => c.State))
                .ForMember(x => x.City, opt => opt.MapFrom(c => c.City))
                .ForMember(x => x.AddressLine1, opt => opt.MapFrom(c => c.AddressLine1))
                .ForMember(x => x.AddressLine2, opt => opt.MapFrom(c => c.AddressLine2))
                .ReverseMap();
        }
    }
}
