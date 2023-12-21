using MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Profiling
{
    public class UserProfileVM
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public List<AddressEntityVM> Addresses { get; set; }
    }
}
