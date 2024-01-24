using Volo.Abp.Domain.Entities;

namespace MicroStore.Bff.Shopping.Data.Profiling
{
    public class UserProfile : AuditiedEntity<string>
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public string? Avatar { get; set; }
        public List<Address> Addresses { get; set; }
    }
}
