using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Dtos;
using MicroStore.IdentityProvider.Identity.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.IdentityProvider.Identity.Application.Roles
{
    public interface IRoleCommandService : IApplicationService
    {
        Task<Result<IdentityRoleDto>> CreateAsync(RoleModel model, CancellationToken cancellationToken = default);
        Task<Result<IdentityRoleDto>> UpdateAsync(string roleId, RoleModel model, CancellationToken cancellationToken = default);
    }
}
