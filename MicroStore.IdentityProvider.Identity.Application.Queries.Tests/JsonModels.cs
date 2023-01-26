namespace MicroStore.IdentityProvider.Identity.Application.Queries.Tests
{
    public class UserReadModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public List<ClaimReadModel>   UserClaims { get; set; }
        public List<RoleReadModel> UserRoles { get; set; }
    }


    public class ClaimReadModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }


    public class RoleReadModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

    }
   
}
