using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.Abstraction.Commands;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Domain.Entities;
using System.Net;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Shipping.Application.Commands
{
    public class CreateShipmentCommandHandler : CommandHandler<CreateShipmentCommand>
    {
        private readonly IRepository<Shipment> _shipmentRepository;

        public CreateShipmentCommandHandler(IRepository<Shipment> shipmentRepository)
        {
            _shipmentRepository = shipmentRepository;
        }

        public override async Task<ResponseResult> Handle(CreateShipmentCommand request, CancellationToken cancellationToken)
        {
            var isOrderShipmentCreated = await _shipmentRepository.AnyAsync(x=> x.OrderId == request.OrderId);

            if(isOrderShipmentCreated)
            {
                return ResponseResult.Failure((int)HttpStatusCode.BadRequest, new ErrorInfo
                {
                    Message = $"Shipment is already created for Order with id : {request.OrderId}"
                });
            }

            Shipment shipment = new Shipment(request.OrderId, request.UserId, request.Address.AsAddress())
            {
                Items = request.Items.Select(x => x.AsShipmentItem()).ToList(),
            };
          
            await _shipmentRepository.InsertAsync(shipment);

            return  ResponseResult.Success((int) HttpStatusCode.Created, ObjectMapper.Map<Shipment, ShipmentDto>(shipment));
        }
    }
}
