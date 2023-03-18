using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.IdentityProvider.Identity.Application.Roles
{
    public interface IRoleQueryService : IApplicationService
    {
        Task<ResultV2<List<IdentityRoleDto>>> ListAsync(CancellationToken cancellationToken = default);
        Task<ResultV2<IdentityRoleDto>> GetAsync(string roleId , CancellationToken cancellationToken  =default);
        Task<ResultV2<IdentityRoleDto>> GetByNameAsync(string roleName, CancellationToken cancellationToken = default);
    }
}
