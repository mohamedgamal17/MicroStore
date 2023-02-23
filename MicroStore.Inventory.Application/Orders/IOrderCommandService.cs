using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.Inventory.Application.Orders
{
    public interface IOrderCommandService : IApplicationService
    {
        Task<UnitResult<OrderDto>> AllocateOrderStockAsync(AllocateOrderStockModel model, CancellationToken cancellationToken = default);

        Task<UnitResult<OrderDto>> ReleaseOrderStockAsync(string orderId , CancellationToken cancellationToken = default);
    }


}
