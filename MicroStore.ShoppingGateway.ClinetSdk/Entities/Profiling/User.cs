using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling;

namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Profiling
{
    public class User : BaseEntity<string>
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }


        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public List<Address> Addresses { get; set; }
    }
}
