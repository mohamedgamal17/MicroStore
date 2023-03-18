using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.Inventory.Application.Products
{
    public interface IProductCommandService : IApplicationService
    {
        Task<ResultV2<ProductDto>> CreateAsync(ProductModel model, CancellationToken cancellationToken = default);

        Task<ResultV2<ProductDto>> UpdateAsync( ProductModel model , CancellationToken cancellationToken = default);

        Task<ResultV2<ProductDto>> AdjustInventory(string id, AdjustProductInventoryModel model  , CancellationToken cancellationToken = default);
    }
}
