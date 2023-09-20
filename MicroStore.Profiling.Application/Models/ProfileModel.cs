using MicroStore.Profiling.Application.Domain;
namespace MicroStore.Profiling.Application.Models
{

    public class ProfileModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public PhoneModel Phone { get; set; }
        public string? Avatar { get; set; }
        public List<Address>? Addresses { get; set; }
    }

    public class CreateProfileModel : ProfileModel
    {
        public string UserId { get; set; }
    }
}
