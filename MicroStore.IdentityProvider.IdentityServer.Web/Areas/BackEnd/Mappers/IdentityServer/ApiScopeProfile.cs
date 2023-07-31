using AutoMapper;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models;
using MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.ApiScopes;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Mappers.IdentityServer
{
    public class ApiScopeProfile : Profile
    {
        public ApiScopeProfile()
        {
            CreateMap<CreateOrEditApiScopeModel, ApiScopeModel>();

            CreateMap<ApiScopeDto, CreateOrEditApiScopeModel>();

            CreateMap<ApiScopePropertyDto, PropertyViewModel>()
               .ForMember(x => x.ParentId, opt => opt.MapFrom(src => src.ScopeId))
               .ForMember(x => x.PropertyId, opt => opt.MapFrom(src => src.Id));
        }

    }
}
