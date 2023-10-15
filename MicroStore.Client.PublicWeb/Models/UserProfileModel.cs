using MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling;
using System.ComponentModel.DataAnnotations;

namespace MicroStore.Client.PublicWeb.Models
{
    public class UserProfileModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public IFormFile? Avatar { get; set; }
    }
}
