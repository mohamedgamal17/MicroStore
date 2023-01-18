using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Uow;
namespace MicroStore.Shipping.Application.Abstraction.Common
{
    public interface IShipmentSystemProvider : IUnitOfWorkEnabled
    {
        string SystemName { get; }
        Task<ResponseResult<ShipmentDto>>  Fullfill(Guid shipmentId, FullfillModel model , CancellationToken  cancellationToken =default);
        Task<ResponseResult<ShipmentDto>> BuyShipmentLabel(string externalShipmentId, BuyShipmentLabelModel model, CancellationToken cancellationToken  = default);
        Task<ResponseResult<ListResultDto<ShipmentRateDto>>> RetriveShipmentRates(string externalShipmentId, CancellationToken cancellationToken = default);
        Task<ResponseResult<ListResultDto<EstimatedRateDto>>> EstimateShipmentRate(EstimatedRateModel model, CancellationToken cancellationToken = default);
        Task<ResponseResult<AddressValidationResultModel>> ValidateAddress(AddressModel addressModel, CancellationToken cancellation = default);
   
    }
}
