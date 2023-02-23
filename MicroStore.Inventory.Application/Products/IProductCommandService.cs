using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.Inventory.Application.Products
{
    public interface IProductCommandService : IApplicationService
    {
        Task<UnitResult<ProductDto>> CreateAsync(ProductModel model, CancellationToken cancellationToken = default);

        Task<UnitResult<ProductDto>> UpdateAsync( ProductModel model , CancellationToken cancellationToken = default);

        Task<UnitResult<ProductDto>> AdjustInventory(string id, AdjustProductInventoryModel model  , CancellationToken cancellationToken = default);
    }
}
