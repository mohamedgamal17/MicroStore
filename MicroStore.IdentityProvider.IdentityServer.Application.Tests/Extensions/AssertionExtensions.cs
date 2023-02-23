using Duende.IdentityServer.EntityFramework.Entities;
using FluentAssertions;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests.Extensions
{
    public static class AssertionExtensions
    {

        public static void AssertApiResourceCommand(this ApiResource apiResource , ApiResourceModel model)
        {
            apiResource.Name.Should().Be(model.Name);
            apiResource.Description.Should().Be(model.Description);
            apiResource.DisplayName.Should().Be(model.DisplayName);
            apiResource.RequireResourceIndicator.Should().Be(apiResource.RequireResourceIndicator);
            apiResource.ShowInDiscoveryDocument.Should().Be(apiResource.ShowInDiscoveryDocument);
            apiResource.Enabled.Should().Be(apiResource.Enabled);
            apiResource.Scopes?.OrderBy(x=> x.Scope).Should().Equal(model.Scopes?.OrderBy(x=> x), (left, right) =>
            {
                return left.Scope == right;
            });

            apiResource.UserClaims?.OrderBy(x=> x.Type).Should().Equal(model.UserClaims?.OrderBy(x=>x), (left, right) =>
            {
                return left.Type == right;
            });

            apiResource.Properties.OrderBy(x=> x.Key).Should().Equal(model.Properties?.OrderBy(x => x.Key), (left, right) =>
            {
                return left.Key == right.Key &&
                left.Value == right.Value;
            });
        }


        public static void AssertApiScopeCommand(this ApiScope apiScope, ApiScopeModel model)
        {

            apiScope.Name.Should().Be(apiScope.Name);
            apiScope.Description.Should().Be(model.Description);
            apiScope.DisplayName.Should().Be(model.DisplayName);
            apiScope.ShowInDiscoveryDocument.Should().Be(apiScope.ShowInDiscoveryDocument);
            apiScope.Enabled.Should().Be(apiScope.Enabled);
            apiScope.Emphasize.Should().Be(apiScope.Emphasize);

            apiScope.UserClaims?.OrderBy(x=>x.Type).Should().Equal(model.UserClaims?.OrderBy(x=>x), (left, right) =>
            {
                return left.Type == right;
            });

            apiScope.Properties?.OrderBy(x=>x.Key).Should().Equal(model.Properties?.OrderBy(x => x), (left, right) =>
            {
                return left.Key == right.Key &&
                left.Value == right.Value;
            });

        }

        public static void AssertClientCommand(this Client client , ClientModel model)
        {

            client.ClientId.Should().Be(model.ClientId);
            client.ClientUri.Should().Be(model.ClientUri);
            client.ClientName.Should().Be(model.ClientName);
            client.Description.Should().Be(model.Description);
            client.ClientClaimsPrefix.Should().Be(model.ClientClaimsPrefix);
            client.LogoUri.Should().Be(model.LogoUri);


            client.IdentityTokenLifetime.Should().Be(model.IdentityTokenLifetime);
            client.AccessTokenType.Should().Be((int)model.AccessTokenType);
            client.AccessTokenLifetime.Should().Be(model.AccessTokenLifetime);
            client.UpdateAccessTokenClaimsOnRefresh.Should().Be(model.UpdateAccessTokenClaimsOnRefresh);
            client.AllowAccessTokensViaBrowser.Should().Be(model.AllowAccessTokensViaBrowser);
            client.AlwaysIncludeUserClaimsInIdToken.Should().Be(model.AlwaysIncludeUserClaimsInIdToken);


            client.RefreshTokenUsage.Should().Be((int)model.RefreshTokenUsage);
            client.RefreshTokenExpiration.Should().Be((int)model.RefreshTokenExpiration);
            client.AbsoluteRefreshTokenLifetime.Should().Be(model.AbsoluteRefreshTokenLifetime);
            client.SlidingRefreshTokenLifetime.Should().Be(model.SlidingRefreshTokenLifetime);



            client.BackChannelLogoutSessionRequired.Should().Be(model.BackChannelLogoutSessionRequired);
            client.BackChannelLogoutUri.Should().Be(model.BackChannelLogoutUri);
            client.FrontChannelLogoutSessionRequired.Should().Be(model.FrontChannelLogoutSessionRequired);
            client.FrontChannelLogoutUri.Should().Be(model.FrontChannelLogoutUri);

            client.RequireConsent.Should().Be(model.RequireConsent);
            client.RequireClientSecret.Should().Be(model.RequireClientSecret);
            client.RequirePkce.Should().Be(model.RequirePkce);
            client.RequireRequestObject.Should().Be(model.RequireRequestObject);

      

    
            client.AllowPlainTextPkce.Should().Be(model.AllowPlainTextPkce);
            client.AllowOfflineAccess.Should().Be(model.AllowOfflineAccess);
            client.AllowRememberConsent.Should().Be(model.AllowRememberConsent);
            client.ConsentLifetime.Should().Be(model.ConsentLifetime);
            client.AlwaysSendClientClaims.Should().Be(model.AlwaysSendClientClaims);
            client.AuthorizationCodeLifetime.Should().Be(model.AuthorizationCodeLifetime);
            client.AlwaysSendClientClaims.Should().Be(model.AlwaysSendClientClaims);
            client.PollingInterval.Should().Be(model.PollingInterval);
            client.PairWiseSubjectSalt.Should().Be(model.PairWiseSubjectSalt);
            client.IncludeJwtId.Should().Be(model.IncludeJwtId);
            client.CibaLifetime.Should().Be(model.CibaLifetime);

            if (model.AllowedIdentityTokenSigningAlgorithms.Any())
            {
                client.AllowedIdentityTokenSigningAlgorithms.Should().Be(model.AllowedIdentityTokenSigningAlgorithms.JoinAsString(","));
            }

            client.AllowedGrantTypes?.OrderBy(x => x.GrantType).Should().Equal(model.AllowedGrantTypes?.OrderBy(x => x), (left, right) =>
            {
                return left.GrantType == right;
            });

            client.RedirectUris?.OrderBy(x => x.RedirectUri).Should().Equal(model.RedirectUris?.OrderBy(x => x), (left, right) =>
            {
                return left.RedirectUri == right;
 
            });

            client.PostLogoutRedirectUris?.OrderBy(x => x.PostLogoutRedirectUri).Should().Equal(model.PostLogoutRedirectUris?.OrderBy(x => x), (left, right) =>
            {
                return left.PostLogoutRedirectUri == right;

            });

            client.AllowedScopes?.OrderBy(x => x.Scope).Should().Equal(model.AllowedScopes?.OrderBy(x => x), (left, right) =>
            {
                return left.Scope == right;
            });

            client.Claims?.OrderBy(x => x.Type).Should().Equal(model.Claims?.OrderBy(x => x.Type), (left, right) =>
            {
                return left.Type == right.Type &&
                left.Value == right.Value;

            });

            client.Properties?.OrderBy(x => x.Id).Should().Equal(model.Properties?.OrderBy(x => x.Key), (left, right) =>
            {
                return left.Key == right.Key &&
                left.Value == right.Value;

            });

        }




        public static void AssertApiResource(this ApiResource apiResource , ApiResourceDto apiResourceDto)
        {
            apiResource.Id.Should().Be(apiResourceDto.Id);
            apiResource.Name.Should().Be(apiResourceDto.Name);
            apiResource.Description.Should().Be(apiResourceDto.Description);
            apiResource.DisplayName.Should().Be(apiResourceDto.DisplayName);
            apiResource.AllowedAccessTokenSigningAlgorithms.Should().Be(apiResourceDto.AllowedAccessTokenSigningAlgorithms);
            apiResource.RequireResourceIndicator.Should().Be(apiResource.RequireResourceIndicator);
            apiResource.ShowInDiscoveryDocument.Should().Be(apiResource.ShowInDiscoveryDocument);
            apiResource.Enabled.Should().Be(apiResource.Enabled);
            apiResource.Scopes?.OrderBy(x => x.Id).Should().Equal(apiResourceDto.Scopes?.OrderBy(x => x.Id), (left, right) =>
            {
                return left.Scope == right.Scope &&
                left.Id == right.Id &&
                left.ApiResourceId == left.ApiResourceId;
            });

            apiResource.UserClaims?.OrderBy(x=> x.Id).Should().Equal(apiResourceDto.UserClaims?.OrderBy(x => x.Id), (left, right) =>
            {
                return left.Type == right.Type &&
                left.Id == right.Id &&
                left.ApiResourceId == right.ApiResourceId;
            });

            apiResource.Properties?.OrderBy(x => x.Id).Should().Equal(apiResourceDto.Properties?.OrderBy(x => x.Id), (left, right) =>
            {
                return left.Key == right.Key &&
                left.Value == right.Value &&
                left.Id == right.Id &&
                left.ApiResourceId == right.ApiResourceId;
            });
        }


        public static void AssertApiScope(this ApiScope apiScope  , ApiScopeDto apiScopeDto)
        {
            apiScope.Id.Should().Be(apiScopeDto.Id);
            apiScope.Name.Should().Be(apiScope.Name);
            apiScope.Description.Should().Be(apiScopeDto.Description);
            apiScope.DisplayName.Should().Be(apiScopeDto.DisplayName);
            apiScope.ShowInDiscoveryDocument.Should().Be(apiScope.ShowInDiscoveryDocument);
            apiScope.Enabled.Should().Be(apiScope.Enabled);
            apiScope.Emphasize.Should().Be(apiScope.Emphasize);

            apiScope.UserClaims?.OrderBy(x => x.Id).Should().Equal(apiScopeDto.UserClaims?.OrderBy(x => x.Id), (left, right) =>
            {
                return left.Type == right.Type &&
                left.Id == right.Id &&
                left.ScopeId == right.ScopeId;
            });

            apiScope.Properties?.OrderBy(x => x.Id).Should().Equal(apiScopeDto.Properties?.OrderBy(x => x.Id), (left, right) =>
            {
                return left.Key == right.Key &&
                left.Value == right.Value &&
                left.Id == right.Id &&
                left.ScopeId == right.ScopeId;
            });
        }


        public static void AssertClient(this Client client , ClientDto clientDto)
        {
            client.Id.Should().Be(clientDto.Id);
            client.ClientId.Should().Be(clientDto.ClientId);
            client.ClientUri.Should().Be(clientDto.ClientUri);
            client.ClientName.Should().Be(clientDto.ClientName);
            client.Description.Should().Be(clientDto.Description);
            client.ClientClaimsPrefix.Should().Be(clientDto.ClientClaimsPrefix);
            client.LogoUri.Should().Be(clientDto.LogoUri);


            client.IdentityTokenLifetime.Should().Be(clientDto.IdentityTokenLifetime);
            client.AccessTokenType.Should().Be(clientDto.AccessTokenType);
            client.AccessTokenLifetime.Should().Be(clientDto.AccessTokenLifetime);
            client.UpdateAccessTokenClaimsOnRefresh.Should().Be(clientDto.UpdateAccessTokenClaimsOnRefresh);
            client.AllowAccessTokensViaBrowser.Should().Be(clientDto.AllowAccessTokensViaBrowser);
            client.AlwaysIncludeUserClaimsInIdToken.Should().Be(clientDto.AlwaysIncludeUserClaimsInIdToken);


            client.RefreshTokenUsage.Should().Be(clientDto.RefreshTokenUsage);
            client.RefreshTokenExpiration.Should().Be(clientDto.RefreshTokenExpiration);
            client.AbsoluteRefreshTokenLifetime.Should().Be(clientDto.AbsoluteRefreshTokenLifetime);
            client.SlidingRefreshTokenLifetime.Should().Be(clientDto.SlidingRefreshTokenLifetime);



            client.BackChannelLogoutSessionRequired.Should().Be(clientDto.BackChannelLogoutSessionRequired);
            client.BackChannelLogoutUri.Should().Be(clientDto.BackChannelLogoutUri);
            client.FrontChannelLogoutSessionRequired.Should().Be(clientDto.FrontChannelLogoutSessionRequired);
            client.FrontChannelLogoutUri.Should().Be(clientDto.FrontChannelLogoutUri);

            client.RequireConsent.Should().Be(clientDto.RequireConsent);
            client.RequireClientSecret.Should().Be(clientDto.RequireClientSecret);
            client.RequirePkce.Should().Be(clientDto.RequirePkce);
            client.RequireRequestObject.Should().Be(clientDto.RequireRequestObject);


            client.AllowedIdentityTokenSigningAlgorithms.Should().Be(clientDto.AllowedIdentityTokenSigningAlgorithms);
            client.AllowPlainTextPkce.Should().Be(clientDto.AllowPlainTextPkce);
            client.AllowOfflineAccess.Should().Be(clientDto.AllowOfflineAccess);
            client.AllowRememberConsent.Should().Be(clientDto.AllowRememberConsent);
            client.ConsentLifetime.Should().Be(clientDto.ConsentLifetime);
            client.AlwaysSendClientClaims.Should().Be(clientDto.AlwaysSendClientClaims);
            client.AuthorizationCodeLifetime.Should().Be(clientDto.AuthorizationCodeLifetime);
            client.AlwaysSendClientClaims.Should().Be(clientDto.AlwaysSendClientClaims);
            client.ProtocolType.Should().Be(clientDto.ProtocolType);
            client.PollingInterval.Should().Be(clientDto.PollingInterval);
            client.PairWiseSubjectSalt.Should().Be(clientDto.PairWiseSubjectSalt);
            client.IncludeJwtId.Should().Be(clientDto.IncludeJwtId);
            client.CibaLifetime.Should().Be(clientDto.CibaLifetime);


            client.UserCodeType.Should().Be(clientDto.UserCodeType);

            client.UserSsoLifetime.Should().Be(clientDto.UserSsoLifetime);

            client.AllowedGrantTypes?.OrderBy(x => x.Id).Should().Equal(clientDto.AllowedGrantTypes?.OrderBy(x => x.Id), (left, right) =>
            {
                return left.Id == right.Id &&
                left.GrantType == right.GrantType &&
                left.ClientId == right.ClientId;
            });

            client.RedirectUris?.OrderBy(x => x.Id).Should().Equal(clientDto.RedirectUris?.OrderBy(x => x.Id), (left, right) =>
            {
                return left.RedirectUri == right.RedirectUri &&
                left.Id == right.Id &&
                left.ClientId == right.ClientId;
            });

            client.PostLogoutRedirectUris?.OrderBy(x => x.Id).Should().Equal(clientDto.PostLogoutRedirectUris?.OrderBy(x => x.Id), (left, right) =>
            {
                return left.PostLogoutRedirectUri == right.PostLogoutRedirectUri &&
                left.Id == right.Id &&
                left.ClientId == right.ClientId;
            });

            client.AllowedScopes?.OrderBy(x => x.Id).Should().Equal(clientDto.AllowedScopes?.OrderBy(x => x.Id), (left, right) =>
            {
                return left.Scope == right.Scope &&
                left.Id == right.Id &&
                left.ClientId == right.ClientId;
            });

            client.Claims?.OrderBy(x => x.Id).Should().Equal(clientDto.Claims?.OrderBy(x => x.Id), (left, right) =>
            {
                return left.Type == right.Type &&
                left.Value == right.Value &&
                left.Id == right.Id &&
                left.ClientId == right.ClientId;
            });

            client.Properties?.OrderBy(x => x.Id).Should().Equal(clientDto.Properties?.OrderBy(x => x.Id), (left, right) =>
            {
                return left.Key == right.Key &&
                left.Value == right.Value &&
                left.Id == right.Id &&
                left.ClientId == right.ClientId;
            });
                
        }
    }
}
