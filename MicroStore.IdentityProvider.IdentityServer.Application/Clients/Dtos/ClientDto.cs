#pragma warning disable 8618
using Duende.IdentityServer.EntityFramework.Entities;
using Volo.Abp.Application.Dtos;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Clients.Dtos
{
    public class ClientDto : EntityDto<int>
    {
        public bool Enabled { get; set; }
        public string ClientId { get; set; }
        public string ProtocolType { get; set; } 
        public List<ClientSecretDto> ClientSecrets { get; set; }
        public bool RequireClientSecret { get; set; } = true;
        public string ClientName { get; set; }
        public string Description { get; set; }
        public string ClientUri { get; set; }
        public string LogoUri { get; set; }
        public bool RequireConsent { get; set; } = false;
        public bool AllowRememberConsent { get; set; } = true;
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
        public List<ClientGrantTypeDto> AllowedGrantTypes { get; set; }
        public bool RequirePkce { get; set; } = true;
        public bool AllowPlainTextPkce { get; set; }
        public bool RequireRequestObject { get; set; }
        public bool AllowAccessTokensViaBrowser { get; set; }
        public List<ClientRedirectUriDto> RedirectUris { get; set; }
        public List<ClientPostLogoutRedirectUriDto> PostLogoutRedirectUris { get; set; }
        public string FrontChannelLogoutUri { get; set; }
        public bool FrontChannelLogoutSessionRequired { get; set; } 
        public string BackChannelLogoutUri { get; set; }
        public bool BackChannelLogoutSessionRequired { get; set; }
        public bool AllowOfflineAccess { get; set; }
        public List<ClientScopeDto> AllowedScopes { get; set; }
        public int IdentityTokenLifetime { get; set; }
        public string AllowedIdentityTokenSigningAlgorithms { get; set; }
        public int AccessTokenLifetime { get; set; }
        public int AuthorizationCodeLifetime { get; set; }
        public int? ConsentLifetime { get; set; } = null;
        public int AbsoluteRefreshTokenLifetime { get; set; } 
        public int SlidingRefreshTokenLifetime { get; set; }
        public int RefreshTokenUsage { get; set; } 
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
        public int RefreshTokenExpiration { get; set; } 
        public int AccessTokenType { get; set; } 
        public bool EnableLocalLogin { get; set; } 
        public List<ClientIdPRestrictionDto> IdentityProviderRestrictions { get; set; }
        public bool IncludeJwtId { get; set; }
        public List<ClientClaimDto> Claims { get; set; }
        public bool AlwaysSendClientClaims { get; set; }
        public string ClientClaimsPrefix { get; set; } 
        public string PairWiseSubjectSalt { get; set; }
        public List<ClientCorsOriginDto> AllowedCorsOrigins { get; set; }
        public List<ClientProperty> Properties { get; set; }
        public int? UserSsoLifetime { get; set; }
        public string UserCodeType { get; set; }
        public int DeviceCodeLifetime { get; set; }
        public int? CibaLifetime { get; set; }
        public int? PollingInterval { get; set; }
        public bool? CoordinateLifetimeWithUserSession { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? LastAccessed { get; set; }
        public bool NonEditable { get; set; }
    }
}
