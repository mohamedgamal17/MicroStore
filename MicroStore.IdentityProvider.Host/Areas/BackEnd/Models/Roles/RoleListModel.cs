using MicroStore.IdentityProvider.Identity.Application.Dtos;

namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Models.Roles
{
    public class RoleListModel : BaseListModel
    {
        public List<IdentityRoleDto> Data { get; set; }
    }
}
