using AutoMapper;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Geographic;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Geographic;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Geographic;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Mappers.Geographic
{
    public class GeographicProfile :Profile
    {
        public GeographicProfile()
        {
            CreateMap<Country, CountryVM>()
                .ForMember(x => x.StateProvinces, opt => opt.MapFrom(c => c.StateProvinces));

            CreateMap<Country, CountryModel>();

            CreateMap<CountryModel, CountryRequestOptions>();

            CreateMap<StateProvince, StateProvinceVM>();

            CreateMap<StateProvince, StateProvinceModel>();

            CreateMap<StateProvinceModel, StateProvinceRequestOptions>();
        }
    }
}
