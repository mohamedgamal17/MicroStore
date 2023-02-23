using AutoMapper;
using Duende.IdentityServer.EntityFramework.Entities;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Mappers
{
    public class ClientProfile : Profile
    {

        public ClientProfile()
        {
            CreateMap<Client, ClientDto>()
                .ForMember(x => x.RedirectUris, opt => opt.MapFrom(c => c.RedirectUris))
                .ForMember(x => x.PostLogoutRedirectUris, opt => opt.MapFrom(c => c.PostLogoutRedirectUris))
                .ForMember(x => x.ClientSecrets, opt => opt.MapFrom(c => c.ClientSecrets))
                .ForMember(x => x.AllowedScopes, opt => opt.MapFrom(c => c.AllowedScopes))
                .ForMember(x => x.Claims, opt => opt.MapFrom(c => c.Claims))
                .ForMember(x => x.AllowedCorsOrigins, opt => opt.MapFrom(c => c.AllowedCorsOrigins))
                .ForMember(x => x.IdentityProviderRestrictions, opt => opt.MapFrom(c => c.IdentityProviderRestrictions))
                .ForMember(x => x.AllowedGrantTypes, opt => opt.MapFrom(c => c.AllowedGrantTypes));


            CreateMap<ClientRedirectUri, ClientRedirectUriDto>();
            CreateMap<ClientPostLogoutRedirectUri, ClientPostLogoutRedirectUriDto>();
            CreateMap<ClientSecret, ClientSecretDto>();
            CreateMap<ClientScope, ClientScopeDto>();
            CreateMap<ClientClaim, ClientClaimDto>();
            CreateMap<ClientCorsOrigin, ClientCorsOriginDto>();
            CreateMap<ClientIdPRestriction, ClientIdPRestrictionDto>();
            CreateMap<ClientGrantType, ClientGrantTypeDto>();


            CreateMap<ClientModel, Client>()
                .ForMember(x => x.AllowedGrantTypes, opt => opt.MapFrom(x => x.AllowedGrantTypes))
                .ForMember(x => x.RedirectUris, opt => opt.MapFrom(c => c.RedirectUris))
                .ForMember(x => x.PostLogoutRedirectUris, opt => opt.MapFrom(c => c.PostLogoutRedirectUris))
                .ForMember(x => x.AllowedIdentityTokenSigningAlgorithms, opt => opt.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter, x => x.AllowedIdentityTokenSigningAlgorithms))
                .ForMember(x => x.IdentityProviderRestrictions, opt => opt.MapFrom(c => c.IdentityProviderRestrictions))
                .ForMember(x => x.Claims, opt => opt.MapFrom(c => c.Claims))
                .ForMember(x => x.AllowedCorsOrigins, opt => opt.MapFrom(c => c.AllowedCorsOrigins))
                .ForMember(x => x.Properties, opt => opt.MapFrom(c => c.Properties));


            CreateMap<PropertyModel, ClientProperty>()
                .ForMember(x => x.Key, opt => opt.MapFrom(c => c.Key))
                .ForMember(x => x.Value, opt => opt.MapFrom(c => c.Value));

            CreateMap<string, ClientCorsOrigin>()
                .ForMember(x => x.Origin, opt => opt.MapFrom(src => src));

            CreateMap<string, ClientIdPRestriction>()
                .ForMember(x => x.Provider, opt => opt.MapFrom(src => src));


            CreateMap<ClaimModel, ClientClaim>()
                .ForMember(x => x.Type, opt => opt.MapFrom(c => c.Type))
                .ForMember(x => x.Value, opt => opt.MapFrom(c => c.Value));

            CreateMap<string, ClientScope>()
                .ForMember(x => x.Scope, opt => opt.MapFrom(src => src));

            CreateMap<string, ClientPostLogoutRedirectUri>()
                .ForMember(x => x.PostLogoutRedirectUri, opt => opt.MapFrom(src => src));

            CreateMap<string, ClientRedirectUri>()
                .ForMember(x => x.RedirectUri, opt => opt.MapFrom(src => src));

            CreateMap<string, ClientGrantType>()
                 .ForMember(x => x.GrantType, opt => opt.MapFrom(src => src));
        }
    }
}
