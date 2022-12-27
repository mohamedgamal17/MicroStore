using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
namespace MicroStore.Ordering.Application.Abstractions.Commands
{
    public class CompleteOrderCommand : ICommandV1
    {
        public Guid OrderId { get; set; }
        public DateTime ShipedDate { get; set; }
    }
}
