using AutoMapper;
using Duende.IdentityServer.EntityFramework.Entities;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes.Dtos;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Mappers
{
    public class ApiScopeMapper : Profile
    {
        public ApiScopeMapper()
        {
            CreateMap<ApiScope, ApiScopeDto>()
                .ForMember(x => x.Properties, opt => opt.MapFrom(c => c.Properties))
                .ForMember(x => x.UserClaims, opt => opt.MapFrom(c => c.UserClaims));


            CreateMap<ApiScopeProperty, ApiScopePropertyDto>();
            CreateMap<ApiScopeClaim, ApiScopeClaimDto>();
        }
    }
}
