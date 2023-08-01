using MicroStore.IdentityProvider.Identity.Domain.Shared.Dtos;
using MicroStore.IdentityProvider.Identity.Web.Areas.BackEnd.Models;

namespace MicroStore.IdentityProvider.Identity.Web.Areas.BackEnd.Models.Roles
{
    public class RoleListViewModel : ListModel
    {
        public List<IdentityRoleDto> Data { get; set; }
    }
}
