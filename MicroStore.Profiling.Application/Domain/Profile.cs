using Volo.Abp.Domain.Entities;

namespace MicroStore.Profiling.Application.Domain
{
    public class Profile : Entity<string>
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public Phone Phone { get; set; }
        public string? Avatar { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>();

        public Profile()
        {
            Id  = Guid.NewGuid().ToString();
        }
    }
}
