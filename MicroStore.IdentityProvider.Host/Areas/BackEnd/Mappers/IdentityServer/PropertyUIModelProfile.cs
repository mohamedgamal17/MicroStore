using AutoMapper;
using MicroStore.IdentityProvider.Host.Areas.BackEnd.Models;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;

namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Mappers.IdentityServer
{
    public class PropertyUIModelProfile : Profile
    {
        public PropertyUIModelProfile()
        {
            CreateMap<PropertyUIModel, PropertyModel>();
                
        }
    }
}
