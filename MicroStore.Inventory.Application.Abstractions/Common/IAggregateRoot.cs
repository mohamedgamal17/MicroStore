using MicroStore.Inventory.Events.Contracts;

namespace MicroStore.Inventory.Application.Abstractions.Common
{
    public interface IAggregateRoot
    {
        public Guid CorrelationId { get; }
        ICurrentState GetCurrentState();
        void Recive(IEvent domainEvent);
        void Recive(ICurrentState currentState);
        IReadOnlyList<IEvent> GetUncomittedEvents();
        IReadOnlyList<IEvent> GetEvents();

    }


  
}
