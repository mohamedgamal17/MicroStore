using MicroStore.Inventory.Events.Contracts;

namespace MicroStore.Inventory.Application.Abstractions.Common
{
    public abstract class AggregateRoot : IAggregateRoot
    {
        public Guid CorrelationId { get; }

        private readonly List<IEvent> _allEvents;

        private readonly List<IEvent> _uncommitedEvents;

        public AggregateRoot(Guid correlationId)
        {
            CorrelationId = correlationId;
            _allEvents = new List<IEvent>();
            _uncommitedEvents = new List<IEvent>();
        }

        protected void Append(IEvent domainEvent)
        {
            ApplyEvent(domainEvent);

            _uncommitedEvents.Add(domainEvent);

            _allEvents.Add(domainEvent);
       
        }
       
        public IReadOnlyList<IEvent> GetEvents()
        {
            return _allEvents.ToList();
        }

        public IReadOnlyList<IEvent> GetUncomittedEvents()
        {
            return _uncommitedEvents.ToList();
        }

        public void Recive(IEvent domainEvent)
        {
            _allEvents.Add(domainEvent);

            ApplyEvent(domainEvent);
        }

        protected abstract void ApplyEvent(IEvent domainEvent);

        public abstract ICurrentState GetCurrentState();

        public abstract void Recive(ICurrentState currentState);



    }
}
