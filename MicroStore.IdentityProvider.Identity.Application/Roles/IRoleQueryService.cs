using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.IdentityProvider.Identity.Application.Roles
{
    public interface IRoleQueryService : IApplicationService
    {
        Task<Result<List<IdentityRoleDto>>> ListAsync(CancellationToken cancellationToken = default);
        Task<Result<IdentityRoleDto>> GetAsync(string roleId , CancellationToken cancellationToken  =default);
        Task<Result<IdentityRoleDto>> GetByNameAsync(string roleName, CancellationToken cancellationToken = default);
    }
}
