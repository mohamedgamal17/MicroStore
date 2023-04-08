using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.Inventory.Application.Products
{
    public interface IProductCommandService : IApplicationService
    {
        Task<Result<ProductDto>> CreateAsync(ProductModel model, CancellationToken cancellationToken = default);

        Task<Result<ProductDto>> UpdateProductInfoAsync( ProductModel model , CancellationToken cancellationToken = default);

        Task<Result<ProductDto>> UpdateProductAsync(string id, InventoryItemModel model  , CancellationToken cancellationToken = default);
    }
}
