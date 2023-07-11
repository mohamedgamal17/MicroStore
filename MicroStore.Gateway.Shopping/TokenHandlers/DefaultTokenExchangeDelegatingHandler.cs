using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using MicroStore.Gateway.Shopping.Config;
using MicroStore.Gateway.Shopping.Security;
using MicroStore.Gateway.Shopping.Services;

namespace MicroStore.Gateway.Shopping.TokenHandlers
{
    public class DefaultTokenExchangeDelegatingHandler : BaseTokenExchangeDelegatingHandler
    {
        const string CacheKey = "mvcgatewaytodownstreamtokenexchangeclient";

        public DefaultTokenExchangeDelegatingHandler(IHttpClientFactory httpClinetFactory, IClientAccessTokenCache clientAccessTokenCache, IOptions<IdentityProviderOptions> identityProviderOptions, IOptions<GatewayClientOptions> gatewayClientOptions, ILogger<DefaultTokenExchangeDelegatingHandler> logger, HttpContextClaimsPrincibalAccessor claimsPrincibalAccessor, IHttpContextAccessor httpContextAccessor) : base(httpClinetFactory, clientAccessTokenCache, identityProviderOptions, gatewayClientOptions, logger, claimsPrincibalAccessor)
        {
            HttpContext = httpContextAccessor.HttpContext!;
        }

        protected override List<string> RequiredScopes => ShoppingGatewayScopes.List();

        protected HttpContext HttpContext { get; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if(request.Headers.Authorization?.Parameter != null)
            {
                var incomingToken = request.Headers.Authorization!.Parameter!;

                request.SetBearerToken(await GetAccessToken(incomingToken, CacheKey, cancellationToken));
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
