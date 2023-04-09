using Volo.Abp.Application.Dtos;
namespace MicroStore.IdentityProvider.Identity.Application.Dtos
{
    public class IdentityUserDto : EntityDto<string>
    {
        public string? GivenName { get; set; }
        public string? FamilyName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public List<IdentityUserRoleDto> UserRoles { get; set; }

    }
}
