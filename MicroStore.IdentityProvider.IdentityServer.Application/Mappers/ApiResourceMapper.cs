using AutoMapper;
using Duende.IdentityServer.EntityFramework.Entities;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiResources.Dtos;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Mappers
{
    public class ApiResourceMapper : Profile
    {
        public ApiResourceMapper()
        {
            CreateMap<ApiResource, ApiResourceDto>()
                .ForMember(x => x.Secrets, opt => opt.MapFrom(c => c.Secrets))
                .ForMember(x => x.Scopes, opt => opt.MapFrom(c => c.Scopes))
                .ForMember(x => x.Properties, opt => opt.MapFrom(c => c.Properties))
                .ForMember(x => x.UserClaims, opt => opt.MapFrom(c => c.UserClaims));

            CreateMap<ApiResourceSecret, ApiResourceSecretDto>();
            CreateMap<ApiResourceScope, ApiResourceScopeDto>();
            CreateMap<ApiResourceProperty, ApiResourcePropertyDto>();
            CreateMap<ApiResourceClaim, ApiResourceClaimDto>();


        }
    }
}
