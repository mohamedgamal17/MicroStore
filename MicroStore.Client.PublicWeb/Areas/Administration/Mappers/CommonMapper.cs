using AutoMapper;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Common;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Mappers
{
    public class CommonMapper : Profile
    {
        public CommonMapper()
        {
            CreateMap<Dimension, DimensionModel>().ReverseMap();

            CreateMap<Weight, WeightModel>().ReverseMap();

            CreateMap<AddressCountry, AddressCountryVM>();

            CreateMap<AddressStateProvince, AddressStateProvinceVM>();

            CreateMap<AddressAggregate, AddressVM>()
                .ForMember(x => x.Country, opt => opt.MapFrom(c => c.Country))
                .ForMember(x => x.State, opt => opt.MapFrom(c => c.State));

            CreateMap<Address, AddressModel>()
                .ForMember(x => x.StateProvince, opt => opt.MapFrom(c => c.State))
                .ForMember(x => x.CountryCode, opt => opt.MapFrom(c => c.CountryCode));

            CreateMap<AddressModel, Address>()
                .ForMember(x => x.State, opt => opt.MapFrom(c => c.StateProvince))
                .ForMember(x => x.CountryCode, opt => opt.MapFrom(c => c.CountryCode));
        }
    }
}
