using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.Inventory.Application.Products
{
    public interface IProductCommandService : IApplicationService
    {

        Task<Result<ProductDto>> CreateOrUpdateAsync(string id, InventoryItemModel model  , CancellationToken cancellationToken = default);
    }
}
