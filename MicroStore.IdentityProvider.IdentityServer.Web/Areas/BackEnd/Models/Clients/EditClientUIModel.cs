using Duende.IdentityServer.Models;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.Clients
{
    public class EditClientUIModel
    {
        public int Id { get; set; }

        #region Client_Information_Section
        public bool Enabled { get; set; } = true;
        public string ClientId { get; set; }
        public string? ClientName { get; set; }
        public string? Description { get; set; }
        public string? ClientUri { get; set; }
        public string? LogoUri { get; set; }
        public bool RequireConsent { get; set; } = false;
        public bool AllowRememberConsent { get; set; } = true;
        public bool RequireClientSecret { get; set; } = true;
        #endregion

        #region Client_Uris_Section
        public string? RedirectUris { get; set; }
        public string? PostLogoutRedirectUris { get; set; }
        public string? AllowedCorsOrigins { get; set; }
        #endregion


        #region Client_Resources_Section
        public List<string>? AllowedScopes { get; set; }

        #endregion



        #region Client_Advanced_Section

        public List<string>? AllowedGrantTypes { get; set; }

        public int IdentityTokenLifetime { get; set; } = 300;

        public int AccessTokenLifetime { get; set; } = 3600;

        public int AuthorizationCodeLifetime { get; set; } = 300;

        public int AbsoluteRefreshTokenLifetime { get; set; } = 2592000;

        public int SlidingRefreshTokenLifetime { get; set; } = 1296000;

        public int? ConsentLifetime { get; set; } = null;

        public int? UserSsoLifetime { get; set; }

        public string? FrontChannelLogoutUri { get; set; }

        public bool FrontChannelLogoutSessionRequired { get; set; } = true;

        public string? BackChannelLogoutUri { get; set; }

        public bool BackChannelLogoutSessionRequired { get; set; } = true;

        public string? PairWiseSubjectSalt { get; set; }

        public AccessTokenType AccessTokenType { get; set; } = AccessTokenType.Jwt;

        public bool IncludeJwtId { get; set; } = true;

        public bool RequirePkce { get; set; } = true;

        public bool AllowPlainTextPkce { get; set; } = false;

        public bool AlwaysIncludeUserClaimsInIdToken { get; set; } = false;

        #endregion



    }
}
