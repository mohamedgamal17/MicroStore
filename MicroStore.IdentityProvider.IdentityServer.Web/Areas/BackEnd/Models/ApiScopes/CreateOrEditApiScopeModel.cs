using MicroStore.IdentityProvider.IdentityServer.Application.Models;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.ApiScopes
{
    public class CreateOrEditApiScopeModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public bool Emphasize { get; set; }

    }


}
