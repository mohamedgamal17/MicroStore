using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.Models;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Models;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Clients
{
    public abstract class ClientCommand
    {
        #region ClinetInformation
        public string ClientName { get; set; }
        public string? Description { get; set; }
        public string ClientUri { get; set; }
        public string? LogoUri { get; set; }
        public List<string>? AllowedRedirectUris { get; set; }
        public List<string>? PostLogoutRedirectUris { get; set; }
        #endregion

        #region ConsentArea
        public bool AllowRememberConsent { get; set; }
        public bool RequireConsent { get; set; }
        public int? ConsentLifetime { get; set; } = null;
        #endregion


        #region FrontChannel
        public bool FrontChannelLogoutSessionRequired { get; set; } = true;
        public string? FrontChannelLogoutUri { get; set; }
        #endregion

        #region BackChannel
        public bool BackChannelLogoutSessionRequired { get; set; } = true;
        public string? BackChannelLogoutUri { get; set; }
        #endregion

        #region IdentityToken
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
        public int IdentityTokenLifetime { get; set; } = 300;
        #endregion

        #region AccessToken
        public AccessTokenType AccessTokenType { get; set; } = AccessTokenType.Jwt;
        public int AccessTokenLifetime { get; set; } = 3600;
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
        #endregion

        #region RefreshToken
        public TokenExpiration RefreshTokenExpiration { get; set; } = TokenExpiration.Absolute;
        public TokenUsage RefreshTokenUsage { get; set; } = TokenUsage.OneTimeOnly;
        public int SlidingRefreshTokenLifetime { get; set; } = 1296000;
        public int AbsoluteRefreshTokenLifetime { get; set; } = 2592000;
        #endregion

        #region AdvancedConfiguration
        public bool AllowOfflineAccess { get; set; }
        public bool RequireClientSecret { get; set; }
        public bool RequirePkce { get; set; }
        public List<string>? AllowedCorsOrigins { get; set; }
        public List<string>? AllowedGrantTypes { get; set; }
        public List<string>? AllowedScopes { get; set; }
        public List<PropertyModel>? Properties { get; set; }
        #endregion



    }


    public class CreateClientCommand : ClientCommand, ICommand<ClientDto>
    {
    }


    public class UpdateClientCommand : ClientCommand, ICommand<ClientDto>
    {
        public int ClientId { get; set; }

    }

    public class RemoveClientCommand : ICommand
    {
        public int ClinetId { get; set; }
    }


    public class AddClientSecretCommand : SecretModel, ICommand<ClientDto>
    {
        public int ClinetId { get; set; }

    }

    public class RemoveClientSecretCommand : ICommand<ClientDto>
    {
        public int ClinetId { get; set; }
        public int SecretId { get; set; }
    }

}
