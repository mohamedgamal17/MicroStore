using MicroStore.IdentityProvider.IdentityServer.Application.Clients.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Common.Dtos;

namespace MicroStore.IdentityProvider.IdentityServer.Application.ApiResources.Dtos
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
        public int ApiResourceId { get; set;}
    }

    public class ApiResourceScopeDto : ScopeDto<int>
    {
        public int ApiResourceId { get; set;}
    }
}
