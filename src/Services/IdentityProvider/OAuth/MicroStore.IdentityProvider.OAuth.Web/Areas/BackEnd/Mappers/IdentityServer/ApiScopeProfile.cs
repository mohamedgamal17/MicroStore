using AutoMapper;
using MicroStore.IdentityProvider.OAuth.Application.Dtos;
using MicroStore.IdentityProvider.OAuth.Application.Models;
using MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models;
using MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Models.ApiScopes;

namespace MicroStore.IdentityProvider.OAuth.Web.Areas.BackEnd.Mappers.IdentityServer
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
