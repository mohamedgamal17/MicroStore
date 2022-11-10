using MicroStore.Inventory.Events.Contracts;

namespace MicroStore.Inventory.Application.Abstractions.Common
{
    public interface IAggregateRoot
    {
        public Guid CorrelationId { get; }
        void Recive(IEvent domainEvent);
        IReadOnlyList<IEvent> GetUncomittedEvents();
        IReadOnlyList<IEvent> GetEvents();

    }
}
