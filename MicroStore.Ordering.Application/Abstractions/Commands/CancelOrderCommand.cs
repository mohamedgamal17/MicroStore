#nullable disable
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
namespace MicroStore.Ordering.Application.Abstractions.Commands
{
    public class CancelOrderCommand : ICommandV1
    {
        public Guid OrderId { get; set; }
        public string Reason { get; set; }
        public DateTime CancellationDate { get; set; }
    }
}
