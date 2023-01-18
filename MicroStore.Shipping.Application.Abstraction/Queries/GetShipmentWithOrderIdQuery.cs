using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Shipping.Application.Abstraction.Dtos;

namespace MicroStore.Shipping.Application.Abstraction.Queries
{
    public class GetShipmentWithOrderIdQuery :IQuery<ShipmentDto>
    {
        public string OrderId { get; set; }

    }
}
