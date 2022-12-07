#nullable disable
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Ordering.Events.Models;
namespace MicroStore.Ordering.Application.Abstractions.Commands
{
    public class FullfillOrderCommand : ICommand
    {
        public Guid OrderId { get; set; }
        public string ShipmentId { get; set; }
        public string ShipmentSystem { get; set; }
    }
}
