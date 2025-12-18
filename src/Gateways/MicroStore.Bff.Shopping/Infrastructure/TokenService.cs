using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Security.Claims;
using IdentityModel.Client;
using Newtonsoft.Json;
using System.Net.Http;
using MicroStore.Bff.Shopping.Config;
using Microsoft.AspNetCore.Authentication;

namespace MicroStore.Bff.Shopping.Infrastructure
{
    public class TokenService : ITokenService
    {
        const string CacheKey = "mvcbffaggregatortodownstreamtokenexchangeclient";

        private readonly HttpContext _httpContext;

        private readonly IClientAccessTokenCache _clientAccessTokenCache;

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly SecurityConfiguration _securityConfiguration;

        private readonly ILogger<TokenService> _logger;

        public TokenService(IHttpContextAccessor httpContextAccessor, IClientAccessTokenCache clientAccessTokenCache, IHttpClientFactory httpClientFactory, SecurityConfiguration gatewayClientConfiguration, ILogger<TokenService> logger)
        {
            _httpContext = httpContextAccessor.HttpContext!;
            _clientAccessTokenCache = clientAccessTokenCache;
            _httpClientFactory = httpClientFactory;
            _securityConfiguration = gatewayClientConfiguration;
            _logger = logger;
        }

        public async Task<string> GetAccessToken(CancellationToken cancellationToken = default)
        {
            bool isAuthenticated = _httpContext.User.Identity?.IsAuthenticated ?? false;

            if (!isAuthenticated)
            {
                return string.Empty;
            }

            var tokenParam = new ClientAccessTokenParameters
            {
                Resource = string.Format("clinetId:{0}_sub:{1}", FindClaim(JwtClaimTypes.ClientId)?.Value ?? string.Empty, FindClaim(JwtClaimTypes.Subject)?.Value ?? string.Empty)
            };

           // _logger.LogDebug("Access token cache key : {TokenCacheKey}", tokenParam.Resource);

            var cachedItem = await _clientAccessTokenCache.GetAsync(CacheKey, tokenParam, cancellationToken);

            if (cachedItem != null)
            {
                return cachedItem.AccessToken!;
            }

            string incomingToken = (await _httpContext.GetUserAccessTokenAsync())!;

            var (accessToken, expiresIn) = await ExchangeToken(incomingToken, cancellationToken); ;

            await _clientAccessTokenCache.SetAsync(CacheKey, accessToken, expiresIn, tokenParam, cancellationToken);

            return accessToken;
        }

        protected async Task<(string accessToken, int expiresIn)> ExchangeToken(string incomingToken, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();

            var discoveryDocumentResponse = await client.GetDiscoveryDocumentAsync(_securityConfiguration.Jwt.Authority, cancellationToken);

            if (discoveryDocumentResponse.IsError)
            {
                throw new InvalidOperationException(discoveryDocumentResponse.Error);
            }

            var customParams = new Parameters
            {
                { "subject_token_type", "urn:ietf:params:oauth:token-type:access_token"},
                { "subject_token", incomingToken},
                { "scope", BffAggregatorScopes.List().Aggregate((x1 , x2)=> x1 + " " + x2) }
            };

            var dict = new Dictionary<string, object>()
            {
                {"subject_token_type", "urn:ietf:params:oauth:token-type:access_token"},
                {"subject_token", incomingToken},
                {"scope", BffAggregatorScopes.List().Aggregate((x1 , x2)=> x1 + " " + x2) },
                {"clinet_id", _securityConfiguration.DownStreamClient.Id },
                {"client_secret", _securityConfiguration.DownStreamClient.Secret},
                {"grant_type",  "urn:ietf:params:oauth:grant-type:token-exchange" }

            };

            var json = JsonConvert.SerializeObject(dict);

            var result = await client.PostAsync(discoveryDocumentResponse.TokenEndpoint, new StringContent(json), cancellationToken);


            var tokenResponse = await client.RequestTokenAsync(new TokenRequest()
            {
                Address = discoveryDocumentResponse.TokenEndpoint,
                GrantType = "urn:ietf:params:oauth:grant-type:token-exchange",
                Parameters = customParams,
                ClientId = _securityConfiguration.DownStreamClient.Id,
                ClientSecret = _securityConfiguration.DownStreamClient.Secret
            }, cancellationToken);

            if (tokenResponse.IsError)
            {
                throw new InvalidOperationException(tokenResponse.Error);
            }

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Token Exchanged : {Token}", tokenResponse.AccessToken);
            }

            return (tokenResponse.AccessToken, tokenResponse.ExpiresIn);
        }


        public Claim? FindClaim(string claimType)
        {
            return _httpContext.User?.Claims.FirstOrDefault(x => x.Type == claimType);
        }
    }


    public interface ITokenService
    {
        Task<string> GetAccessToken(CancellationToken cancellationToken = default);
    }
}
