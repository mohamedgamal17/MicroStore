using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.Inventory.Application.Products
{
    public interface IProductQueryService : IApplicationService
    {
        Task<Result<PagedResult<ProductDto>>> ListAsync(PagingQueryParams queryParams , CancellationToken cancellationToken = default);

        Task<Result<ProductDto>> GetAsync(string id , CancellationToken cancellationToken = default);

        Task<Result<ProductDto>> GetBySkyAsync(string sku, CancellationToken cancellationToken = default);

        Task<Result<PagedResult<ProductDto>>> Search(ProductSearchModel model, CancellationToken cancellationToken = default);
    }
}
