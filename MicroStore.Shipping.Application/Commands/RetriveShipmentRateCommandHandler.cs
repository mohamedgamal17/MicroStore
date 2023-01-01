using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Extensions;
using System.Net;
namespace MicroStore.Shipping.Application.Commands
{
    public class RetriveShipmentRateCommandHandler : QueryHandler<RetriveShipmentRateCommand>
    {
        private readonly IShipmentSystemResolver _shipmentSystemResolver;

        public RetriveShipmentRateCommandHandler(IShipmentSystemResolver shipmentSystemResolver)
        {
            _shipmentSystemResolver = shipmentSystemResolver;
        }

        public override async Task<ResponseResult> Handle(RetriveShipmentRateCommand request, CancellationToken cancellationToken)
        {

            var unitresult = await _shipmentSystemResolver.Resolve(request.SystemName, cancellationToken);

            if (unitresult.IsFailure)
            {

                return unitresult.ConvertFaildUnitResult();

            }

            var system = unitresult.Value;

            return await system.RetriveShipmentRates(request.ExternalShipmentId);
        }


        
    }
}
