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
        public string Avatar { get; set; }
        public List<AddressModel>? Addresses { get; set; }

        public UserProfileModel()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            BirthDate = DateTime.MinValue;
            Gender = default(Gender);
            Phone = string.Empty;
            Avatar = string.Empty;
        }
    }

    public class CreateUserProfileModel : UserProfileModel
    {
        public string UserId { get; set; }

        public CreateUserProfileModel()
        {
            UserId = string.Empty;
        }
    }
}
