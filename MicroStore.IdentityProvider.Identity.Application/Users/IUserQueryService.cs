using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Domain.Shared.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.IdentityProvider.Identity.Application.Users
{
    public interface IUserQueryService : IApplicationService
    {
        Task<Result<PagedResult<IdentityUserDto>>> ListAsync(PagingQueryParams queryParams, CancellationToken cancellationToken = default);
        Task<Result<IdentityUserDto>> GetAsync(string userId, CancellationToken cancellationToken = default);
        Task<Result<IdentityUserDto>> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
        Task<Result<IdentityUserDto>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
