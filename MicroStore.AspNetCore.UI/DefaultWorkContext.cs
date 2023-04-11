using IdentityModel;
using Microsoft.AspNetCore.Http;

namespace MicroStore.AspNetCore.UI
{
    public class DefaultWorkContext : IWorkContext
    {
        const string AnonymousUser = "anonymous_userid";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public DefaultWorkContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string TryToGetCurrentUserId()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context?.User.Identity?.IsAuthenticated == true)
            {
                return context.User.Claims.Single(x => x.Type == JwtClaimTypes.Subject).Value;

            }


            string? userId;

            if (context?.Request.Cookies.TryGetValue(AnonymousUser, out userId) == true && userId != null)
            {
                return userId;
            }

            userId = Guid.NewGuid().ToString();

            context?.Response.Cookies.Append(AnonymousUser, userId);

            return userId;
        }
    }
}
