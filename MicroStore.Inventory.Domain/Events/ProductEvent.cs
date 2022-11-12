namespace MicroStore.Inventory.Domain.Events
{
    public abstract class EventBase
    {
        public Guid ProductId { get; init; }

    }
}
