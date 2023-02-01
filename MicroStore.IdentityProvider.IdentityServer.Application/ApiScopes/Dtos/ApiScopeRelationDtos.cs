using MicroStore.IdentityProvider.IdentityServer.Application.Common.Dtos;
namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes.Dtos
{
    public class ApiScopePropertyDto : PropertyDto<int>
    {
        public int ScopeId { get; set; }

    }

    public class ApiScopeClaimDto : UserClaimDto<int>
    {
        public int ScopeId { get; set; }

    }
}
