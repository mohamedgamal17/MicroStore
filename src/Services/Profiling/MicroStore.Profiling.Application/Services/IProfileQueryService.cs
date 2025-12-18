using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.BuildingBlocks.Utils.Paging.Params;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Profiling.Application.Dtos;

namespace MicroStore.Profiling.Application.Services
{
    public interface IProfileQueryService
    {
        Task<Result<PagedResult<ProfileDto>>> ListAsync(PagingQueryParams queryParams, CancellationToken cancellationToken = default);
        Task<Result<List<ProfileDto>>> ListByIdsAsync(List<string> ids, CancellationToken cancellationToken = default);
        Task<Result<ProfileDto>> GetAsync(string userId, CancellationToken cancellationToken =  default);
        Task<Result<List<AddressDto>>> ListAddressesAsync(string  userId, CancellationToken cancellationToken = default);
        Task<Result<AddressDto>> GetAddressAsync(string userId, string addressId ,CancellationToken cancellationToken = default);
    }
}
