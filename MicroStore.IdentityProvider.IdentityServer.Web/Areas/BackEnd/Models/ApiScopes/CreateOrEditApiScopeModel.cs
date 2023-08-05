using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using System.ComponentModel.DataAnnotations;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.ApiScopes
{
    public class CreateOrEditApiScopeModel
    {
        public int? Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? DisplayName { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public bool Emphasize { get; set; }

    }


}
