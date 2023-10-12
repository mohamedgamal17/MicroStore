using IdentityModel;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;

namespace MicroStore.BuildingBlocks.AspNetCore.Security
{
    public class ApplicationCurrentUser : ITransientDependency
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
