using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using System.ComponentModel.DataAnnotations;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.ApiResources
{
    public class CreateOrEditApiResourceModel
    {
        public int Id { get; set; }

        [MaxLength(200)]
        [Required]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? DisplayName { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public bool RequireResourceIndicator { get; set; }
        public List<string>? UserClaims { get; set; }
        public List<string>? Scopes { get; set; }
    }
}
