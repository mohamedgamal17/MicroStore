using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using Volo.Abp.Uow;
namespace MicroStore.Shipping.Application.Abstraction.Common
{
    public interface IShipmentSystemProvider : IUnitOfWorkEnabled
    {
        string SystemName { get; }
        Task<UnitResult<ShipmentDto>>  Fullfill(string shipmentId, FullfillModel model , CancellationToken  cancellationToken =default);
        Task<UnitResult<ShipmentDto>> BuyShipmentLabel(string shipmentId, BuyShipmentLabelModel model, CancellationToken cancellationToken  = default);
        Task<UnitResult<List<ShipmentRateDto>>> RetriveShipmentRates(string shipmentId, CancellationToken cancellationToken = default);
        Task<UnitResult<List<EstimatedRateDto>>> EstimateShipmentRate(AddressModel from , AddressModel to , List<ShipmentItemEstimationModel> items, CancellationToken cancellationToken = default);
        Task<UnitResult<AddressValidationResultModel>> ValidateAddress(AddressModel addressModel, CancellationToken cancellation = default);
    }
}
