using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using Volo.Abp.Uow;
namespace MicroStore.Shipping.Application.Abstraction.Common
{
    public interface IShipmentSystemProvider : IUnitOfWorkEnabled
    {
        string SystemName { get; }
        Task<ResultV2<ShipmentDto>>  Fullfill(string shipmentId, FullfillModel model , CancellationToken  cancellationToken =default);
        Task<ResultV2<ShipmentDto>> BuyShipmentLabel(string shipmentId, BuyShipmentLabelModel model, CancellationToken cancellationToken  = default);
        Task<ResultV2<List<ShipmentRateDto>>> RetriveShipmentRates(string shipmentId, CancellationToken cancellationToken = default);
        Task<ResultV2<List<EstimatedRateDto>>> EstimateShipmentRate(AddressModel from , AddressModel to , List<ShipmentItemEstimationModel> items, CancellationToken cancellationToken = default);
        Task<ResultV2<AddressValidationResultModel>> ValidateAddress(AddressModel addressModel, CancellationToken cancellation = default);
    }
}
