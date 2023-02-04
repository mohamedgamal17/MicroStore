namespace MicroStore.IdentityProvider.IdentityServer.Application.Dtos
{
    public class ApiResourceClaimDto : UserClaimDto<int>
    {
        public int ApiResourceId { get; set; }

    }

    public class ApiResourcePropertyDto : PropertyDto<int>
    {
        public int ApiResourceId { get; set; }
    }

    public class ApiResourceSecretDto : SecretDto<int>
    {
        public int ApiResourceId { get; set; }
    }

    public class ApiResourceScopeDto : ScopeDto<int>
    {
        public int ApiResourceId { get; set; }
    }
}
