using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Ordering.Events.Models;
namespace MicroStore.Ordering.Application.Abstractions.Commands
{
    public class CompleteOrderCommand : ICommand
    {
        public Guid OrderId { get; set; }
        public DateTime ShipedDate { get; set; }
    }
}
