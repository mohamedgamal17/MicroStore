using MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Profiling
{
    public class UserProfileVM
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Birth Date")]
        public DateTime BirthDate { get; set; }

        [DisplayName("Gender")]
        public Gender Gender { get; set; }

        [DisplayName("Phone")]
        public string Phone { get; set; }

        [DisplayName("Avatar")]
        public string Avatar { get; set; }
        public List<AddressEntityVM> Addresses { get; set; }
    }
}
