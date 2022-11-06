using IdentityModel;
using System.Security.Claims;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

namespace MicroStore.BuildingBlocks.Security
{
    [Dependency(ReplaceServices = true)]
    internal class ApplicationCurrentUser : ICurrentUser , ITransientDependency 
    {
        public Guid? Id => TryToGetUserId();

        public bool IsAuthenticated => Id.HasValue;

        public string? UserName => FindClaim(JwtClaimTypes.Name)?.Value;

        public string? Name => FindClaim(JwtClaimTypes.GivenName)?.Value;

        public string? SurName => FindClaim(JwtClaimTypes.FamilyName)?.Value;

        public string? PhoneNumber => FindClaim(JwtClaimTypes.PhoneNumber)?.Value;

        public bool PhoneNumberVerified => string.Equals(this.FindClaimValue(JwtClaimTypes.PhoneNumberVerified), "true", StringComparison.InvariantCultureIgnoreCase);

        public string? Email => FindClaim(JwtClaimTypes.Email)?.Value;

        public bool EmailVerified => string.Equals(this.FindClaimValue(JwtClaimTypes.EmailVerified), "true", StringComparison.InvariantCultureIgnoreCase);


        public Guid? TenantId => TryToGetTenantId();

        public string[] Roles => FindClaims(JwtClaimTypes.Role).Select(x => x.Value).ToArray();


        private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

        public ApplicationCurrentUser(ICurrentPrincipalAccessor currentPrincipalAccessor)
        {
            _currentPrincipalAccessor = currentPrincipalAccessor;
        }

        public Claim? FindClaim(string claimType)
        {
            return _currentPrincipalAccessor.Principal.Claims.SingleOrDefault(x => x.Type == claimType);
        }

        public Claim[] FindClaims(string claimType)
        {
            return _currentPrincipalAccessor.Principal.Claims.Where(x => x.Type == claimType).ToArray();
        }

        public Claim[] GetAllClaims()
        {
            return _currentPrincipalAccessor.Principal.Claims.ToArray();
        }

        public bool IsInRole(string roleName)
        {
            return Roles.Any(x => x == roleName);
        }


        private Guid? TryToGetUserId()
        {
            string? userId = _currentPrincipalAccessor.Principal.Claims.SingleOrDefault(x => x.Type == JwtClaimTypes.Subject)?.Value;


            if (Guid.TryParse(userId, out var result))
            {
                return result;
            }

            return null;
        }

        private Guid? TryToGetTenantId()
        {
            string? tenantId = _currentPrincipalAccessor.Principal.Claims.SingleOrDefault(x => x.Type == AbpClaimTypes.TenantId)?.Value;

            if (Guid.TryParse(tenantId, out var result))
            {
                return result;
            }

            return null;
        }
    }
}

