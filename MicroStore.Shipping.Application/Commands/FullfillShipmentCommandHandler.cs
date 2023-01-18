using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Const;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Application.Extensions;
using MicroStore.Shipping.Domain.Entities;
using System.Net;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Shipping.Application.Commands
{
    public class FullfillShipmentCommandHandler : CommandHandler<FullfillShipmentCommand, ShipmentDto>
    {
        private readonly IShipmentSystemResolver _shipmentSystemResolver;

        private readonly IShipmentRepository _shipmentRepository;

        public FullfillShipmentCommandHandler(IShipmentSystemResolver shipmentSystemResolver, IShipmentRepository shipmentRepository)
        {
            _shipmentSystemResolver = shipmentSystemResolver;
            _shipmentRepository = shipmentRepository;
        }

        public override async Task<ResponseResult<ShipmentDto>> Handle(FullfillShipmentCommand request, CancellationToken cancellationToken)
        {
            bool isShipmentExist = await _shipmentRepository.AnyAsync(x=> x.Id == request.ShipmentId, cancellationToken); 

            if(!isShipmentExist)
            {
                return Failure(HttpStatusCode.NotFound, new ErrorInfo
                {
                    Message = $"Shipment entity with id : {request.ShipmentId} is not exist"
                });

            }

            var unitresult = await _shipmentSystemResolver.Resolve(request.SystemName, cancellationToken);

            if (unitresult.IsFailure)
            {
                return unitresult.ConvertFaildUnitResult<ShipmentDto>();
            }

            var system = unitresult.Value;

            var model = new FullfillModel
            {
                AddressFrom = request.AddressFrom,
                Package = request.Pacakge,
            };



            return await system.Fullfill(request.ShipmentId, model);
        }

        
    }
}
