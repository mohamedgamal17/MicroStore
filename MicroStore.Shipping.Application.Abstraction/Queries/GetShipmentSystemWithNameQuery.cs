using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Shipping.Application.Abstraction.Dtos;

namespace MicroStore.Shipping.Application.Abstraction.Queries
{
    public class GetShipmentSystemWithNameQuery :IQuery<ShipmentSystemDto>
    {
        public string Name { get; set; }
    }
}
