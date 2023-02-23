using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.IdentityProvider.Identity.Application.Roles
{
    public interface IRoleQueryService : IApplicationService
    {
        Task<UnitResult<List<IdentityRoleDto>>> ListAsync(CancellationToken cancellationToken = default);
        Task<UnitResult<IdentityRoleDto>> GetAsync(string roleId , CancellationToken cancellationToken  =default);
        Task<UnitResult<IdentityRoleDto>> GetByNameAsync(string roleName, CancellationToken cancellationToken = default);
    }
}
