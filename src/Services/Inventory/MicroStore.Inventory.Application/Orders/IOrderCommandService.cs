using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.Inventory.Application.Orders
{
    public interface IOrderCommandService : IApplicationService
    {
        Task<Result<Unit>> AllocateOrderStockAsync(OrderStockModel model, CancellationToken cancellationToken = default);

        Task<Result<Unit>> ReleaseOrderStockAsync(OrderStockModel model , CancellationToken cancellationToken = default);
    }


}
