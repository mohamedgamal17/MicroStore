using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Application.Dtos;
using Volo.Abp.Application.Services;

namespace MicroStore.Inventory.Application.Products
{
    public interface IProductQueryService : IApplicationService
    {
        Task<UnitResult<PagedResult<ProductDto>>> ListAsync(PagingQueryParams queryParams , CancellationToken cancellationToken = default);

        Task<UnitResult<ProductDto>> GetAsync(string id , CancellationToken cancellationToken = default);

        Task<UnitResult<ProductDto>> GetBySkyAsync(string sku, CancellationToken cancellationToken = default);
    }
}
