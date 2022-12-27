using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
namespace MicroStore.Shipping.Application.Abstraction.Common
{
    public interface IShipmentSystemProvider
    {
        string SystemName { get; }
        Task<ShipmentFullfilledDto>  Fullfill(Guid shipmentId, FullfillModel model , CancellationToken  cancellationToken =default);
        Task<ShipmentDto> BuyShipmentLabel(string externalShipmentId, BuyShipmentLabelModel model, CancellationToken cancellationToken  = default);
        Task<List<ShipmentRateDto>> RetriveShipmentRates(string externalShipmentId);
        Task<List<EstimatedRateDto>> EstimateShipmentRate(EstimatedRateModel model);
        Task<List<CarrierModel>> ListCarriers(CancellationToken cancellationToken = default);
        Task<bool> IsActive(CancellationToken cancellationToken = default);
    }
}
