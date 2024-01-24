using MicroStore.Bff.Shopping.Data.Profiling;
using MicroStore.Bff.Shopping.Models.Common;

namespace MicroStore.Bff.Shopping.Models.Profiling
{
    public class UserProfileModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public string? Avatar { get; set; }
        public List<AddressModel>? Addresses { get; set; }
    }

    public class CreateUserProfileModel : UserProfileModel
    {
        public string UserId { get; set; }
    }
}
