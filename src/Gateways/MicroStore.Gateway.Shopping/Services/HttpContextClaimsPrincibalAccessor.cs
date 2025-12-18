using System.Security.Claims;

namespace MicroStore.Gateway.Shopping.Services
{
    public class HttpContextClaimsPrincibalAccessor
    {

        private readonly IServiceProvider _serviceProvider;

        public HttpContextClaimsPrincibalAccessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public ClaimsPrincipal? TryToGetCurrentClaimsPrincibal()
        {
            var httpContextAccessot = _serviceProvider.GetRequiredService<IHttpContextAccessor>();

            if(httpContextAccessot.HttpContext != null)
            {
                var httpContext = httpContextAccessot.HttpContext;

                return httpContext.User;
            }

            return null;
        }
    }
}
