using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Dtos;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.IdentityProvider.Identity.Application.Roles
{
    public interface IRoleCommandService : IApplicationService
    {
        Task<Result<IdentityRoleDto>> CreateAsync(RoleModel model, CancellationToken cancellationToken = default);
        Task<Result<IdentityRoleDto>> UpdateAsync(string roleId, RoleModel model, CancellationToken cancellationToken = default);
        Task<Result<Unit>> RemoveAsync(string roleId, CancellationToken cancellationToken = default);
    }
}
