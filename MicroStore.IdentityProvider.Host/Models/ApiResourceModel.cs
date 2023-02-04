using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace MicroStore.IdentityProvider.Host.Models
{
    public abstract class ApiResourceModel
    {
        public string Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public bool RequireResourceIndicator { get; set; }
        public List<PropertyModel> Properties { get; set; }
        public List<string> UserClaims { get; set; }
        public List<string> Scopes { get; set; }
    }
 
}
