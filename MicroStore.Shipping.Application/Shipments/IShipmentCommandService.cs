using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.Shipping.Application.Shipments
{
    public interface IShipmentCommandService : IApplicationService
    {
        Task<UnitResultV2<ShipmentDto>> CreateAsync(ShipmentModel model, CancellationToken cancellationToken = default);

        Task<UnitResultV2<ShipmentDto>> FullfillAsync(string shipmentId, PackageModel model, CancellationToken cancellationToken = default);

        Task<UnitResultV2<List<ShipmentRateDto>>> RetriveShipmentRatesAsync(string shipmentId, CancellationToken cancellationToken = default);

        Task<UnitResultV2<ShipmentDto>> BuyLabelAsync(string shipmentId, BuyShipmentLabelModel model, CancellationToken cancellationToken = default);

    }
}
