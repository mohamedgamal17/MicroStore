using AutoMapper;
using MicroStore.IdentityProvider.OAuth.Application.Models;
using MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models;

namespace MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Mappers.IdentityServer
{
    public class PropertyUIModelProfile : Profile
    {
        public PropertyUIModelProfile()
        {
            CreateMap<PropertyViewModel, PropertyModel>();

        }
    }
}
