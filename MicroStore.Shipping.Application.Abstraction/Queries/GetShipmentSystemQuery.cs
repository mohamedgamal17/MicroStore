using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Shipping.Application.Abstraction.Queries
{
    public class GetShipmentSystemQuery :IQuery
    {
        public Guid SystemId { get; set; }
    }
}
