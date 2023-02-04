using Volo.Abp.Application.Dtos;

namespace MicroStore.IdentityProvider.Identity.Application.Dtos
{
    public class IdentityUserListDto : EntityDto<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
