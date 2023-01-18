using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Shipping.Application.Abstraction.Dtos;

namespace MicroStore.Shipping.Application.Abstraction.Queries
{
    public class GetShipmentSystemQuery :IQuery<ShipmentSystemDto>
    {
        public Guid SystemId { get; set; }
    }
}
