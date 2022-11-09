
using MicroStore.Inventory.Events.Contracts;

namespace MicroStore.Inventory.Events.Abstractions
{
    public abstract class Event : IEvent
    {
        public string EventType { get; }

        public DateTime PublishedAt { get; }

        protected Event( string eventType)
        {
            EventType = eventType;
            PublishedAt = DateTime.UtcNow;
        }
    }
}
