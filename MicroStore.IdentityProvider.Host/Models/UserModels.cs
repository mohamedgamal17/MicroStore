using MicroStore.IdentityProvider.Identity.Application.Common.Models;

namespace MicroStore.IdentityProvider.Host.Models
{
    public class UserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
        public List<IdentityClaimModel> Claims { get; set; }
    }

}
