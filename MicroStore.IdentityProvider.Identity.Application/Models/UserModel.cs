#pragma warning disable CS8618

namespace MicroStore.IdentityProvider.Identity.Application.Models
{
    public class UserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Password { get; set; }
        public List<string> UserRoles { get; set; }
        public List<IdentityClaimModel> UserClaims { get; set; }
    }
}
