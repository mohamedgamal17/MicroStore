using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Const;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Application.Extensions;
using System.Net;
namespace MicroStore.Shipping.Application.Commands
{
    public class BuyShipmentLabelCommandHandler : CommandHandlerV1<BuyShipmentLabelCommand>
    {
        private readonly IShipmentSystemResolver _shipmentSystemResolver;

        public BuyShipmentLabelCommandHandler(IShipmentSystemResolver shipmentSystemResolver)
        {
            _shipmentSystemResolver = shipmentSystemResolver;
        }

        public override async Task<ResponseResult> Handle(BuyShipmentLabelCommand request, CancellationToken cancellationToken)
        {
            var unitresult = await _shipmentSystemResolver.Resolve(request.SystemName, cancellationToken);

            if(unitresult.IsFailure)
            {
                return unitresult.ConvertFaildUnitResult();
            }

            var system = unitresult.Value;

            var model = new BuyShipmentLabelModel
            {
                ShipmentRateId = request.RateId,

            };

            var result =  await system.BuyShipmentLabel(request.ExternalShipmentId, model);

            return ResponseResult.Success((int)HttpStatusCode.Accepted, result);
        }

       
    }
}
