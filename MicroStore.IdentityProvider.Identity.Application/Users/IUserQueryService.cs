using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.IdentityProvider.Identity.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.IdentityProvider.Identity.Application.Users
{
    public interface IUserQueryService : IApplicationService
    {
        Task<UnitResultV2<PagedResult<IdentityUserListDto>>> ListAsync(PagingQueryParams queryParams, CancellationToken cancellationToken = default);
        Task<UnitResultV2<IdentityUserDto>> GetAsync(string userI, CancellationToken cancellationToken = default);
        Task<UnitResultV2<IdentityUserDto>> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
        Task<UnitResultV2<IdentityUserDto>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
