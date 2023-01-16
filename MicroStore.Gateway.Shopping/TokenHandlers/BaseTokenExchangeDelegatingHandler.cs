using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using IdentityModel;
using System.Security.Claims;
using MicroStore.Gateway.Shopping.Config;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MicroStore.Gateway.Shopping.TokenHandlers
{
    public abstract class BaseTokenExchangeDelegatingHandler : DelegatingHandler
    {
        protected abstract List<string> RequiredScopes { get;}

        private readonly IHttpClientFactory _httpClinetFactory;

        private readonly IClientAccessTokenCache _clientAccessTokenCache;

        private readonly HttpContext _httpContext;

        private readonly IdentityProviderOptions _identityProviderOptions;

        private readonly GatewayClientOptions _gatewayClientOptions;

        public BaseTokenExchangeDelegatingHandler(IHttpClientFactory httpClinetFactory, IClientAccessTokenCache clientAccessTokenCache, IHttpContextAccessor httpContextAccessor, IOptions<IdentityProviderOptions> identityProviderOptions, IOptions<GatewayClientOptions> gatewayClientOptions)
        {
            _httpClinetFactory = httpClinetFactory;
            _clientAccessTokenCache = clientAccessTokenCache;
            _httpContext = httpContextAccessor.HttpContext!;
            _identityProviderOptions = identityProviderOptions.Value;
            _gatewayClientOptions = gatewayClientOptions.Value;
        }

        protected async Task<string> GetAccessToken(string incomingToken,string cacheKey ,CancellationToken cancellationToken = default)
        {
            var tokenParam = new ClientAccessTokenParameters
            {
                Resource = string.Format("clinetId:{0}_sub:{1}", FindClaim(JwtClaimTypes.ClientId)?.Value ?? string.Empty, FindClaim(JwtClaimTypes.Subject)?.Value ?? string.Empty)
            };


            var cachedItem = await _clientAccessTokenCache.GetAsync(cacheKey, tokenParam, cancellationToken);

            if (cachedItem != null)
            {
                return cachedItem.AccessToken!;
            }


            var (accessToken, expiresIn) = await ExchangeToken(incomingToken, cancellationToken); ;


            await _clientAccessTokenCache.SetAsync(cacheKey, accessToken, expiresIn, tokenParam, cancellationToken);
            return accessToken;
        }

        protected async Task<(string accessToken, int expiresIn)> ExchangeToken(string incomingToken, CancellationToken cancellationToken)
        {
            var client = _httpClinetFactory.CreateClient();

            var discoveryDocumentResponse = await client.GetDiscoveryDocumentAsync(_identityProviderOptions.Authority, cancellationToken);

            if (discoveryDocumentResponse.IsError)
            {
                throw new InvalidOperationException(discoveryDocumentResponse.Error);
            }

            var customParams = new Parameters
            {
                { "subject_token_type", "urn:ietf:params:oauth:token-type:access_token"},
                { "subject_token", incomingToken},
                { "scope", RequiredScopes.Aggregate((x1 , x2)=> x1 + " " + x2) }
            };

            var dict = new Dictionary<string, object>()
            {
                { "subject_token_type", "urn:ietf:params:oauth:token-type:access_token"},
                { "subject_token", incomingToken},
                { "scope", RequiredScopes.Aggregate((x1 , x2)=> x1 + " " + x2) },
                {"clinet_id", _gatewayClientOptions.ClientId },
                {"client_secret", _gatewayClientOptions.ClinetSecret},
                {"grant_type",  "urn:ietf:params:oauth:grant-type:token-exchange" }

            };

            var json = JsonConvert.SerializeObject(dict);

            var result =   await client.PostAsync(discoveryDocumentResponse.TokenEndpoint,new StringContent(json), cancellationToken);


            var tokenResponse = await client.RequestTokenAsync(new TokenRequest()
            {
                Address = discoveryDocumentResponse.TokenEndpoint,
                GrantType = "urn:ietf:params:oauth:grant-type:token-exchange",
                Parameters = customParams,
                ClientId = _gatewayClientOptions.ClientId,
                ClientSecret = _gatewayClientOptions.ClinetSecret
            }, cancellationToken);

            if (tokenResponse.IsError)
            {
                throw new InvalidOperationException(tokenResponse.Error);
            }

            return (tokenResponse.AccessToken, tokenResponse.ExpiresIn);
        }

        public Claim? FindClaim(string claimType)
        {
            return _httpContext.User.Claims.FirstOrDefault(x => x.Type == claimType);
        }
    }
}
