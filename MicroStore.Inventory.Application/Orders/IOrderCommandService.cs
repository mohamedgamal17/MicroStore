using MicroStore.BuildingBlocks.Results;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.Inventory.Application.Orders
{
    public interface IOrderCommandService : IApplicationService
    {
        Task<ResultV2<OrderDto>> AllocateOrderStockAsync(AllocateOrderStockModel model, CancellationToken cancellationToken = default);

        Task<ResultV2<OrderDto>> ReleaseOrderStockAsync(string orderId , CancellationToken cancellationToken = default);
    }


}
