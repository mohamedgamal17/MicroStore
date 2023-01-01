using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using Volo.Abp.Uow;

namespace MicroStore.Shipping.Application.Abstraction.Common
{
    public interface IShipmentSystemProvider : IUnitOfWorkEnabled
    {
        string SystemName { get; }
        Task<ResponseResult>  Fullfill(Guid shipmentId, FullfillModel model , CancellationToken  cancellationToken =default);
        Task<ResponseResult> BuyShipmentLabel(string externalShipmentId, BuyShipmentLabelModel model, CancellationToken cancellationToken  = default);
        Task<ResponseResult> RetriveShipmentRates(string externalShipmentId, CancellationToken cancellationToken = default);
        Task<ResponseResult> EstimateShipmentRate(EstimatedRateModel model, CancellationToken cancellationToken = default);
        Task<ResponseResult> ValidateAddress(AddressModel addressModel, CancellationToken cancellation = default);
   
    }
}
