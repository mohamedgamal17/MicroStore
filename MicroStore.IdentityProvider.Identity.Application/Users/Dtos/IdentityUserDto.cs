using MicroStore.IdentityProvider.Identity.Application.Common.Dtos;
using Volo.Abp.Application.Dtos;

namespace MicroStore.IdentityProvider.Identity.Application.Users.Dtos
{
    public class IdentityUserDto : EntityDto<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public List<IdentityUserRoleDto> UserRoles { get; set; }
        public List<IdentityClaimDto> UserClaims { get; set; }
     
    }
}
