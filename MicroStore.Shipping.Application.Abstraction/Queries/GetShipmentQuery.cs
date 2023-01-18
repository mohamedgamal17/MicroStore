using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Shipping.Application.Abstraction.Dtos;

namespace MicroStore.Shipping.Application.Abstraction.Queries
{
    public class GetShipmentQuery : IQuery<ShipmentDto>
    {
        public Guid ShipmentId { get; set; }
    }
}
