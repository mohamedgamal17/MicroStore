using Volo.Abp.Application.Dtos;

namespace MicroStore.IdentityProvider.Identity.Application.Dtos
{
    public class IdentityUserRoleDto : EntityDto<string>
    {
        public string Name { get; set; }
    }

}
