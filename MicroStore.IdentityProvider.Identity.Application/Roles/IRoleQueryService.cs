using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.IdentityProvider.Identity.Application.Roles
{
    public interface IRoleQueryService : IApplicationService
    {
        Task<UnitResultV2<List<IdentityRoleDto>>> ListAsync(CancellationToken cancellationToken = default);
        Task<UnitResultV2<IdentityRoleDto>> GetAsync(string roleId , CancellationToken cancellationToken  =default);
        Task<UnitResultV2<IdentityRoleDto>> GetByNameAsync(string roleName, CancellationToken cancellationToken = default);
    }
}
