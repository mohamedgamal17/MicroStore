using AutoMapper;
using Duende.IdentityServer.EntityFramework.Entities;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Mappers
{
    public class ApiResourceProfile : Profile
    {
        public ApiResourceProfile()
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

            CreateMap<ApiResourceModel, ApiResource>()
                .ForMember(x => x.Properties, opt => opt.MapFrom(c => c.Properties))
                .ForMember(x => x.UserClaims, opt => opt.MapFrom(c => c.UserClaims))
                .ForMember(x => x.Scopes, opt => opt.MapFrom(c => c.Scopes));

            CreateMap<PropertyModel ,ApiResourceProperty>()
                .ForMember(x => x.Key, opt => opt.MapFrom(c => c.Key))
                .ForMember(x => x.Value, opt => opt.MapFrom(c => c.Value));

            CreateMap<string, ApiResourceClaim>()
                .ForMember(x => x.Type, opt => opt.MapFrom(src => src));

            CreateMap<string, ApiResourceScope>()
                .ForMember(x => x.Scope, opt => opt.MapFrom(src => src));
        }
    }
}
