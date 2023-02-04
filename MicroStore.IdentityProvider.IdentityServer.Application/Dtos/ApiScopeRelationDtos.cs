namespace MicroStore.IdentityProvider.IdentityServer.Application.Dtos
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
