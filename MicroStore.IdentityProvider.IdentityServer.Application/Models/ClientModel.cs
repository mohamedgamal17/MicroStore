#pragma warning disable CS8618
using Duende.IdentityServer.Models;
using FluentValidation;
using System.Text.RegularExpressions;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Models
{
    public class ClientModel
    {
       public string ClientId { get; set; }

        public bool RequireClientSecret { get; set; } = true;

        public string? ClientName { get; set; }

        public string? Description { get; set; }

        public string? ClientUri { get; set; }

        public string? LogoUri { get; set; }

        public bool RequireConsent { get; set; } = false;

        public bool AllowRememberConsent { get; set; } = true;

        public List<string>? AllowedGrantTypes { get; set; }
      
        public bool RequirePkce { get; set; } = true;

        public bool AllowPlainTextPkce { get; set; } = false;

        public bool RequireRequestObject { get; set; } = false;

        public bool AllowAccessTokensViaBrowser { get; set; } = false;

        public List<string>? RedirectUris { get; set; } 

        public List<string>? PostLogoutRedirectUris { get; set; } 

        public string? FrontChannelLogoutUri { get; set; }

        public bool FrontChannelLogoutSessionRequired { get; set; } = true;

        public string? BackChannelLogoutUri { get; set; }

        public bool BackChannelLogoutSessionRequired { get; set; } = true;

        public bool AllowOfflineAccess { get; set; } = false;

        public ICollection<string>? AllowedScopes { get; set; } 

        public bool AlwaysIncludeUserClaimsInIdToken { get; set; } = false;

        public int IdentityTokenLifetime { get; set; } = 300;

        public ICollection<string>? AllowedIdentityTokenSigningAlgorithms { get; set; }

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

        public HashSet<string>? IdentityProviderRestrictions { get; set; }

        public bool IncludeJwtId { get; set; } = true;

        public List<ClaimModel>? Claims { get; set; }

        public bool AlwaysSendClientClaims { get; set; } = false;

        public string ClientClaimsPrefix { get; set; } = "client_";

        public string? PairWiseSubjectSalt { get; set; }

        public int? UserSsoLifetime { get; set; }

        public string? UserCodeType { get; set; }

        public int DeviceCodeLifetime { get; set; } = 300;

        public int? CibaLifetime { get; set; }

        public int? PollingInterval { get; set; }

        public bool? CoordinateLifetimeWithUserSession { get; set; }

        public HashSet<string>? AllowedCorsOrigins { get; set; } 

        public List<PropertyModel>? Properties { get; set; }

        public bool Enabled { get; set; } = true;

    }


    internal class ClientModelValidation : AbstractValidator<ClientModel>
    {
        const string URI_REGEX_PATTERN = @"(https:[/][/]|http:[/][/]|www.)[a-zA-Z0-9\\-\\.]+\\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\\-\\._\\?\\,\\'/\\\\\\+&amp;%\\$#\\=~])*$";
        public ClientModelValidation()
        {
            RuleFor(x => x.ClientId)
                .MaximumLength(200)
                .NotNull();

            RuleFor(x => x.ClientName)
                .MaximumLength(200)
                .When(x => x.ClientName != null);

            RuleFor(x => x.ClientUri)
                .MaximumLength(200)
                .Matches(URI_REGEX_PATTERN)
                .When(x => x.ClientUri != null);

            RuleFor(x => x.LogoUri)
                .MaximumLength(200)
                .Matches(URI_REGEX_PATTERN)
                .When(x => x.ClientUri != null);

            RuleForEach(x => x.AllowedGrantTypes)
                .MaximumLength(200)
                .When(x => x.AllowedGrantTypes != null);

            RuleFor(x => x.IdentityTokenLifetime)
                .GreaterThanOrEqualTo(0);

            RuleForEach(x => x.AllowedIdentityTokenSigningAlgorithms)
                .MaximumLength(200)
                .When(x => x.AllowedIdentityTokenSigningAlgorithms != null);


            RuleFor(x => x.AccessTokenLifetime)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.AuthorizationCodeLifetime)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.AbsoluteRefreshTokenLifetime)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.SlidingRefreshTokenLifetime)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.ConsentLifetime)
               .GreaterThanOrEqualTo(0)
               .When(x => x.ConsentLifetime != null);

            RuleForEach(x => x.IdentityProviderRestrictions)
                .MaximumLength(200)
                .When(x => x.IdentityProviderRestrictions != null);

            RuleForEach(x => x.Claims)
                .SetValidator(new ClaimModelValidator())
                .When(x => x.Claims != null);

            RuleFor(x => x.ClientClaimsPrefix)
                .MaximumLength(200);

            RuleFor(x => x.PairWiseSubjectSalt)
             .MaximumLength(200)
             .When(x => x.PairWiseSubjectSalt != null);

            RuleFor(x => x.UserSsoLifetime)
              .GreaterThan(0)
              .When(x => x.UserSsoLifetime != null);


            RuleFor(x => x.UserCodeType)
              .MaximumLength(200)
              .When(x => x.UserCodeType != null);

            RuleFor(x => x.DeviceCodeLifetime)
              .GreaterThanOrEqualTo(0);

            RuleFor(x => x.CibaLifetime)
               .GreaterThanOrEqualTo(0)
               .When(x => x.CibaLifetime != null);

            RuleFor(x => x.PollingInterval)
              .GreaterThanOrEqualTo(0)
              .When(x => x.PollingInterval != null);


            RuleForEach(x => x.AllowedCorsOrigins)
                .Matches(URI_REGEX_PATTERN)
                .MaximumLength(200)
                .When(x => x.AllowedCorsOrigins != null);

            RuleForEach(x => x.Properties)
                .SetValidator(new PropertyModelValidator())
                .When(x => x.Properties != null);
        }
    }

}
