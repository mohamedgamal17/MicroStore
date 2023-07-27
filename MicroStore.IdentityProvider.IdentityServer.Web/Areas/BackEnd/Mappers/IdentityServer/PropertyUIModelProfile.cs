using AutoMapper;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Mappers.IdentityServer
{
    public class PropertyUIModelProfile : Profile
    {
        public PropertyUIModelProfile()
        {
            CreateMap<PropertyUIModel, PropertyModel>();

        }
    }
}
