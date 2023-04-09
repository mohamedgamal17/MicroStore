#pragma warning disable CS8618

namespace MicroStore.IdentityProvider.Identity.Application.Models
{
    public class UserModel
    {
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Password { get; set; }
        public List<string> UserRoles { get; set; }

    }
}
