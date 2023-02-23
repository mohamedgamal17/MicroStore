using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.Shipping.Application.Shipments
{
    public interface IShipmentCommandService : IApplicationService
    {
        Task<UnitResult<ShipmentDto>> CreateAsync(ShipmentModel model, CancellationToken cancellationToken = default);

        Task<UnitResult<ShipmentDto>> FullfillAsync(string shipmentId, PackageModel model, CancellationToken cancellationToken = default);

        Task<UnitResult<List<ShipmentRateDto>>> RetriveShipmentRatesAsync(string shipmentId, CancellationToken cancellationToken = default);

        Task<UnitResult<ShipmentDto>> BuyLabelAsync(string shipmentId, BuyShipmentLabelModel model, CancellationToken cancellationToken = default);

    }
}
