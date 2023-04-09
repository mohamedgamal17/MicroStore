using Volo.Abp.Application.Dtos;

namespace MicroStore.IdentityProvider.Identity.Application.Dtos
{
    public class IdentityRoleDto : EntityDto<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
