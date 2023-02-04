#pragma warning disable 8618

namespace MicroStore.IdentityProvider.IdentityServer.Application.Dtos
{
    public class ApiResourceDto : ResourceDto<int, ApiResourceClaimDto, ApiResourcePropertyDto>
    {
        public string AllowedAccessTokenSigningAlgorithms { get; set; }
        public bool RequireResourceIndicator { get; set; }
        public List<ApiResourceSecretDto> Secrets { get; set; } = new List<ApiResourceSecretDto>();
        public List<ApiResourceScopeDto> Scopes { get; set; } = new List<ApiResourceScopeDto>();
    }
}
