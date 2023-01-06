using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Shipping.Application.Abstraction.Queries
{
    public class GetShipmentSystemWithNameQuery :IQuery
    {
        public string Name { get; set; }
    }
}
