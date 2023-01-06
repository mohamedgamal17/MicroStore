using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
namespace MicroStore.Ordering.Application.Abstractions.Commands
{
    public class CompleteOrderCommand : ICommand
    {
        public Guid OrderId { get; set; }
        public DateTime ShipedDate { get; set; }
    }
}
