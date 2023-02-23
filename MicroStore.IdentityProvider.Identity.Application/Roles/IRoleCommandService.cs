using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Dtos;
using MicroStore.IdentityProvider.Identity.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.IdentityProvider.Identity.Application.Roles
{
    public interface IRoleCommandService : IApplicationService
    {
        Task<UnitResultV2<IdentityRoleDto>> CreateAsync(RoleModel model, CancellationToken cancellationToken = default);
        Task<UnitResultV2<IdentityRoleDto>> UpdateAsync(string roleId, RoleModel model, CancellationToken cancellationToken = default);
    }
}
