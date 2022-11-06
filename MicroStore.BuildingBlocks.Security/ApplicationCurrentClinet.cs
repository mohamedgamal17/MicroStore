using IdentityModel;
using System.Security.Claims;
using Volo.Abp.Clients;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace MicroStore.BuildingBlocks.Security
{
    [Dependency(ReplaceServices = true)]
    public class ApplicationCurrentClinet : ICurrentClient 
    {
        public string? Id => FindClaim(JwtClaimTypes.ClientId)?.Value;

        public bool IsAuthenticated => Id != null;


        private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

        public ApplicationCurrentClinet(ICurrentPrincipalAccessor currentPrincipalAccessor)
        {
            _currentPrincipalAccessor = currentPrincipalAccessor;
        }

        public Claim? FindClaim(string claimType)
        {
            return _currentPrincipalAccessor.Principal.Claims.SingleOrDefault(x => x.Type == claimType);
        }
    }
}
