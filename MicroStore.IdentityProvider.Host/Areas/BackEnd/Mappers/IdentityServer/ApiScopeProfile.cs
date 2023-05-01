using AutoMapper;
using MicroStore.IdentityProvider.Host.Areas.BackEnd.Models;
using MicroStore.IdentityProvider.Host.Areas.BackEnd.Models.ApiScopes;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;

namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Mappers.IdentityServer
{
    public class ApiScopeProfile : Profile
    {
        public ApiScopeProfile()
        {
            CreateMap<ApiScopeUIModel, ApiScopeModel>();

            CreateMap<ApiScopeDto, ApiScopeUIModel>();

            CreateMap<ApiScopePropertyDto, PropertyUIModel>()
               .ForMember(x => x.ParentId, opt => opt.MapFrom(src => src.ScopeId))
               .ForMember(x => x.PropertyId, opt => opt.MapFrom(src => src.Id));
        }

    }
}
