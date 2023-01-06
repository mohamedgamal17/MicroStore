using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Shipping.Application.Abstraction.Queries
{
    public class GetShipmentWithOrderIdQuery :IQuery
    {
        public string OrderId { get; set; }

    }
}
