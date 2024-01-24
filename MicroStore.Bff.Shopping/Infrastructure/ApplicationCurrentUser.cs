using IdentityModel;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;

namespace MicroStore.Bff.Shopping.Infrastructure
{
    public class ApplicationCurrentUser
    {
        private readonly HttpContext httpContext;
        public ApplicationCurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext!;
        }
        public string? UserId => FindClaimValue(JwtClaimTypes.Subject);
        public string? UserName => FindClaimValue(JwtClaimTypes.Name);
        public string? Email => FindClaimValue(JwtClaimTypes.Email);
        private string? FindClaimValue(string type)
        {
            return httpContext.User?.Claims.SingleOrDefault(x => x.Type == type)?.Value;
        }
    }
}
