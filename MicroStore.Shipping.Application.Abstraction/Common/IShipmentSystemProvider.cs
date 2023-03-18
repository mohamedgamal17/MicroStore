using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using Volo.Abp.Uow;
namespace MicroStore.Shipping.Application.Abstraction.Common
{
    public interface IShipmentSystemProvider : IUnitOfWorkEnabled
    {
        string SystemName { get; }
        Task<Result<ShipmentDto>>  Fullfill(string shipmentId, FullfillModel model , CancellationToken  cancellationToken =default);
        Task<Result<ShipmentDto>> BuyShipmentLabel(string shipmentId, BuyShipmentLabelModel model, CancellationToken cancellationToken  = default);
        Task<Result<List<ShipmentRateDto>>> RetriveShipmentRates(string shipmentId, CancellationToken cancellationToken = default);
        Task<Result<List<EstimatedRateDto>>> EstimateShipmentRate(AddressModel from , AddressModel to , List<ShipmentItemEstimationModel> items, CancellationToken cancellationToken = default);
        Task<Result<AddressValidationResultModel>> ValidateAddress(AddressModel addressModel, CancellationToken cancellation = default);
    }
}
