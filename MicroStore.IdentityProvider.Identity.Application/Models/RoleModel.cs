#pragma warning disable CS8618

namespace MicroStore.IdentityProvider.Identity.Application.Models
{
    public class RoleModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<IdentityClaimModel> Claims { get; set; }
    }
}
