namespace MicroStore.Inventory.Events.Contracts
{
    public interface IEvent
    {       
        public string EventType { get; }
        public DateTime PublishedAt { get;  }

    }
}