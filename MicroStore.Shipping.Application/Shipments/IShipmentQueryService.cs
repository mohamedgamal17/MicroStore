﻿using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.Shipping.Application.Shipments
{
    public interface IShipmentQueryService : IApplicationService
    {
        Task<Result<PagedResult<ShipmentDto>>> ListAsync(ShipmentListQueryModel queryParams,string? userId  = null,CancellationToken cancellationToken = default);
        Task<Result<List<ShipmentDto>>> ListByOrderIds(List<string> orderIds, CancellationToken cancellationToken = default);
        Task<Result<List<ShipmentDto>>> ListByOrderNumbers(List<string> orderNumbers, CancellationToken cancellationToken = default);
        Task<Result<ShipmentDto>> GetAsync(string shipmentId, CancellationToken cancellationToken = default);
        Task<Result <ShipmentDto>> GetByOrderIdAsync(string orderId, CancellationToken cancellationToken = default);
        Task<Result<ShipmentDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<ShipmentDto>>> SearchByOrderNumber(ShipmentSearchByOrderNumberModel model, CancellationToken cancellationToken = default);
    }
}
