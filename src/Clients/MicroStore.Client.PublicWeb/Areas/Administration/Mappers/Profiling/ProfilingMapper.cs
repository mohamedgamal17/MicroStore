using AutoMapper;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Profiling;
using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Profiling;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Mappers.Profiling
{
    public class ProfilingMapper : Profile
    {
        public ProfilingMapper()
        {
            CreateMap<User, UserProfileVM>()
                .ForMember(x => x.Addresses, opt => opt.MapFrom(c => c.Addresses));

            CreateMap<Address, AddressEntityVM>();
                
        }
    }
}
