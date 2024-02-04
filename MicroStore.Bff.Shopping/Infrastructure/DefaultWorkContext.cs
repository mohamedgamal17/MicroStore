using IdentityModel;
using System.Security.Claims;
namespace MicroStore.Bff.Shopping.Infrastructure
{
    public class DefaultWorkContext : IWorkContext
    {
        private readonly HttpContext _httpContext;

        public bool HasUserAuthenticated => _httpContext?.User.Identity?.IsAuthenticated ?? false;
        public AuthenticatedUser? User => TryToGetCurrentUser();

        public DefaultWorkContext(HttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext!;
        }

        private AuthenticatedUser? TryToGetCurrentUser()
        {
            if (HasUserAuthenticated)
            {
                return AuthenticatedUser.FromUserClaims(_httpContext.User);
            }

            return null;
        }
     

    }

    public class AuthenticatedUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string? GivenName { get; set; }
        public string? FamilyName { get; set; }
        public IEnumerable<Claim> Claims { get; set; }

        public static AuthenticatedUser FromUserClaims(ClaimsPrincipal principal)
        {
            var user = new AuthenticatedUser
            {
                Id = principal.Claims.Single(x => x.Type == JwtClaimTypes.Subject).Value,
                UserName = principal.Claims.Single(x => x.Type == JwtClaimTypes.Name).Value,
                GivenName = principal.Claims.SingleOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value,
                FamilyName = principal.Claims.SingleOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value,
                Claims = principal.Claims
            };

            return user;
        }

    }
    public interface IWorkContext
    {
        public bool HasUserAuthenticated { get; }
        public AuthenticatedUser? User { get; }

    }
}
