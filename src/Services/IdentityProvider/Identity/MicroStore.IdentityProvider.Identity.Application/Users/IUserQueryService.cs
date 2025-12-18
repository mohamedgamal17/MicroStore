using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Dtos;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.IdentityProvider.Identity.Application.Users
{
    public interface IUserQueryService : IApplicationService
    {
        Task<Result<PagedResult<IdentityUserDto>>> ListAsync(UserListQueryModel queryParams, CancellationToken cancellationToken = default);
        Task<Result<IdentityUserDto>> GetAsync(string userId, CancellationToken cancellationToken = default);
        Task<Result<IdentityUserDto>> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
        Task<Result<IdentityUserDto>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
