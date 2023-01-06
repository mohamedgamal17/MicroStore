using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Shipping.Application.Abstraction.Queries
{
    public class GetShipmentQuery : IQuery
    {
        public Guid ShipmentId { get; set; }
    }
}
