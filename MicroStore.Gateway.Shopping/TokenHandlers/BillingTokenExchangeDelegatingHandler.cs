using IdentityModel.AspNetCore.AccessTokenManagement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using MicroStore.Gateway.Shopping.Config;
using MicroStore.Gateway.Shopping.Security;
using System.Net.Http.Headers;

namespace MicroStore.Gateway.Shopping.TokenHandlers
{
    public class BillingTokenExchangeDelegatingHandler : BaseTokenExchangeDelegatingHandler
    {
        const string CacheKey = "mvcgatewaytodownstreamtokenexchangeclient_shipping";

        public BillingTokenExchangeDelegatingHandler(IHttpClientFactory httpClinetFactory, IClientAccessTokenCache clientAccessTokenCache,      IHttpContextAccessor httpContextAccessor, IOptions<IdentityProviderOptions> identityProviderOptions, IOptions<GatewayClientOptions> gatewayClientOptions) : base(httpClinetFactory, clientAccessTokenCache, httpContextAccessor, identityProviderOptions, gatewayClientOptions)
        {
        }

        protected override List<string> RequiredScopes => BillingScope.List();

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var incomingToken = request.Headers.Authorization!.Parameter!;

            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, await GetAccessToken(incomingToken, CacheKey, cancellationToken));

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
