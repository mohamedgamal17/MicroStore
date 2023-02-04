using AutoMapper;
using Duende.IdentityServer.EntityFramework.Entities;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Mappers
{
    public class ClientMapper : Profile
    {

        public ClientMapper()
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
        }
    }
}
