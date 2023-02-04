using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.IdentityProvider.Identity.Application.Common.Models;
using MicroStore.IdentityProvider.Identity.Application.Dtos;

namespace MicroStore.IdentityProvider.Identity.Application.Roles
{
    public class RoleCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<IdentityClaimModel> Claims { get; set; }
    }

    public class CreateRoleCommand : RoleCommand, ICommand<IdentityRoleDto>
    {

    }

    public class UpdateRoleCommand : RoleCommand, ICommand<IdentityRoleDto>
    {
        public string RoleId { get; set; }
    }
}
