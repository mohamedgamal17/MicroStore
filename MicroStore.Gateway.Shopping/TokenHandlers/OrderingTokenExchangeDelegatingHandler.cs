using IdentityModel.AspNetCore.AccessTokenManagement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using MicroStore.Gateway.Shopping.Config;
using MicroStore.Gateway.Shopping.Security;
using System.Net.Http.Headers;
namespace MicroStore.Gateway.Shopping.TokenHandlers
{
    public class OrderingTokenExchangeDelegatingHandler : BaseTokenExchangeDelegatingHandler
    {
        const string CacheKey = "mvcgatewaytodownstreamtokenexchangeclient_ordering";
        public OrderingTokenExchangeDelegatingHandler(IHttpClientFactory httpClinetFactory, IClientAccessTokenCache clientAccessTokenCache, IHttpContextAccessor httpContextAccessor, IOptions<IdentityProviderOptions> identityProviderOptions, IOptions<GatewayClientOptions> gatewayClientOptions) : base(httpClinetFactory, clientAccessTokenCache, httpContextAccessor, identityProviderOptions, gatewayClientOptions)
        {
        }

        protected override List<string> RequiredScopes => OrderingScope.List();


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string incomingToken = request.Headers.Authorization?.Parameter!;

            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, await GetAccessToken(incomingToken, CacheKey, cancellationToken));

            return await base.SendAsync(request, cancellationToken);

        }
    }
}
