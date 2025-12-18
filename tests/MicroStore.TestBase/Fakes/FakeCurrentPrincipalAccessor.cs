using IdentityModel;
using System.Security.Claims;
using Volo.Abp;
using Volo.Abp.Security.Claims;

namespace MicroStore.TestBase.Fakes
{
    public class FakeCurrentPrincipalAccessor : ICurrentPrincipalAccessor
    {
        public ClaimsPrincipal Principal => _currentPrincipal;

        private ClaimsPrincipal _currentPrincipal;

        public FakeCurrentPrincipalAccessor()
        {
            ClaimsIdentity identities = new ClaimsIdentity();
            identities.AddClaim(new Claim(JwtClaimTypes.Subject, Guid.NewGuid().ToString()));
            identities.AddClaim(new Claim(JwtClaimTypes.Name, "FakeUserName"));
            identities.AddClaim(new Claim(JwtClaimTypes.GivenName, "FakeUserName"));
            identities.AddClaim(new Claim(JwtClaimTypes.FamilyName, "FakeUserFamilyName"));
            identities.AddClaim(new Claim(JwtClaimTypes.Role, "admin"));
            identities.AddClaim(new Claim(JwtClaimTypes.Role, "user"));

            _currentPrincipal = new ClaimsPrincipal(identities);
        }

        public virtual IDisposable Change(ClaimsPrincipal principal)
        {
            return SetCurrent(principal);
        }

        private IDisposable SetCurrent(ClaimsPrincipal principal)
        {
            var parent = Principal;
            _currentPrincipal = principal;
            return new DisposeAction(() =>
            {
                _currentPrincipal = parent;
            });
        }
    }
}
