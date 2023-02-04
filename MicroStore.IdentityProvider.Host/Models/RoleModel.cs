using MicroStore.IdentityProvider.Identity.Application.Common.Models;

namespace MicroStore.IdentityProvider.Host.Models
{
    public class RoleModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<IdentityClaimModel> Claims { get; set; }
    }
}
