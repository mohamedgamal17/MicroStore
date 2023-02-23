#pragma warning disable CS8618
using AutoMapper;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.Models;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Models
{
    public class ClientModel
    {
        public ClientModel()
        {
            AllowedGrantTypes = new List<string>();
            RedirectUris = new List<string>();
            PostLogoutRedirectUris = new List<string>();
            AllowedScopes = new List<string>();
            AllowedIdentityTokenSigningAlgorithms = new List<string>();
            AllowedCorsOrigins = new List<string>();
            Properties = new List<PropertyModel>();

        }

        public string ClientId { get; set; }

        public bool RequireClientSecret { get; set; } = true;

        public string? ClientName { get; set; }

        public string? Description { get; set; }

        public string? ClientUri { get; set; }

        public string? LogoUri { get; set; }

        public bool RequireConsent { get; set; } = false;

        public bool AllowRememberConsent { get; set; } = true;

        public ICollection<string> AllowedGrantTypes { get; set; }
      
        public bool RequirePkce { get; set; } = true;

        public bool AllowPlainTextPkce { get; set; } = false;

        public bool RequireRequestObject { get; set; } = false;

        public bool AllowAccessTokensViaBrowser { get; set; } = false;

        public ICollection<string> RedirectUris { get; set; } 

        public ICollection<string> PostLogoutRedirectUris { get; set; } 

        public string? FrontChannelLogoutUri { get; set; }

        public bool FrontChannelLogoutSessionRequired { get; set; } = true;

        public string? BackChannelLogoutUri { get; set; }

        public bool BackChannelLogoutSessionRequired { get; set; } = true;

        public bool AllowOfflineAccess { get; set; } = false;

        public ICollection<string> AllowedScopes { get; set; } 

        public bool AlwaysIncludeUserClaimsInIdToken { get; set; } = false;

        public int IdentityTokenLifetime { get; set; } = 300;

        public ICollection<string> AllowedIdentityTokenSigningAlgorithms { get; set; }

        public int AccessTokenLifetime { get; set; } = 3600;

        public int AuthorizationCodeLifetime { get; set; } = 300;

        public int AbsoluteRefreshTokenLifetime { get; set; } = 2592000;

        public int SlidingRefreshTokenLifetime { get; set; } = 1296000;

        public int? ConsentLifetime { get; set; } = null;

        public TokenUsage RefreshTokenUsage { get; set; } = TokenUsage.OneTimeOnly;

        public bool UpdateAccessTokenClaimsOnRefresh { get; set; } = false;

        public TokenExpiration RefreshTokenExpiration { get; set; } = TokenExpiration.Absolute;

        public AccessTokenType AccessTokenType { get; set; } = AccessTokenType.Jwt;

        public bool EnableLocalLogin { get; set; } = true;

        public ICollection<string> IdentityProviderRestrictions { get; set; } = new HashSet<string>();

        public bool IncludeJwtId { get; set; } = true;

        public ICollection<ClaimModel> Claims { get; set; } = new List<ClaimModel>();

        public bool AlwaysSendClientClaims { get; set; } = false;

        public string ClientClaimsPrefix { get; set; } = "client_";

        public string? PairWiseSubjectSalt { get; set; }

        public int? UserSsoLifetime { get; set; }

        public string? UserCodeType { get; set; }

        public int DeviceCodeLifetime { get; set; } = 300;

        public int? CibaLifetime { get; set; }

        public int? PollingInterval { get; set; }

        public bool? CoordinateLifetimeWithUserSession { get; set; }

        public ICollection<string> AllowedCorsOrigins { get; set; } = new HashSet<string>();

        public List<PropertyModel> Properties { get; set; } = new List<PropertyModel>();

        public bool Enabled { get; set; } = true;

    }

}
