using MicroStore.IdentityProvider.IdentityServer.Application.Models;

namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Models.ApiScopes
{
    public class ApiScopeUIModel 
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public bool Emphasize { get; set; }

    }


}
