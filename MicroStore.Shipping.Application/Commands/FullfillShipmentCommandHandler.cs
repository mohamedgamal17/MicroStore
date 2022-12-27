using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Const;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Application.Extensions;
using MicroStore.Shipping.Domain.Entities;
using System.Net;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Shipping.Application.Commands
{
    public class FullfillShipmentCommandHandler : CommandHandlerV1<FullfillShipmentCommand>
    {
        private readonly IShipmentSystemResolver _shipmentSystemResolver;

        private readonly IShipmentRepository _shipmentRepository;

        public FullfillShipmentCommandHandler(IShipmentSystemResolver shipmentSystemResolver, IShipmentRepository shipmentRepository)
        {
            _shipmentSystemResolver = shipmentSystemResolver;
            _shipmentRepository = shipmentRepository;
        }

        public override async Task<ResponseResult> Handle(FullfillShipmentCommand request, CancellationToken cancellationToken)
        {
            bool isShipmentExist = await _shipmentRepository.AnyAsync(x=> x.Id == request.ShipmentId, cancellationToken); 

            if(!isShipmentExist)
            {
                return ResponseResult.Failure((int)HttpStatusCode.NotFound, new ErrorInfo
                {
                    Message = $"Shipment entity with id : {request.ShipmentId} is not exist"
                });

                throw new EntityNotFoundException(typeof(Shipment), request.ShipmentId);
            }

            var unitresult = await _shipmentSystemResolver.Resolve(request.SystemName, cancellationToken);

            if (unitresult.IsFailure)
            {
                return unitresult.ConvertFaildUnitResult();
            }

            var system = unitresult.Value;

            var model = new FullfillModel
            {
                AddressFrom = request.AddressFrom,
                Package = request.Pacakge,
            };

            var result = await system.Fullfill(request.ShipmentId, model);

            return ResponseResult.Success((int)HttpStatusCode.Accepted, result);
        }

        
    }
}
