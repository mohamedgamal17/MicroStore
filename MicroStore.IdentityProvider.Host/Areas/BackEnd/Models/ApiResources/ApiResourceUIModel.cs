using MicroStore.IdentityProvider.IdentityServer.Application.Models;

namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Models.ApiResources
{
    public class ApiResourceUIModel  
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public bool RequireResourceIndicator { get; set; }
        public List<string>? UserClaims { get; set; }
        public List<string>? Scopes { get; set; }
    }
}
