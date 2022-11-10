
namespace MicroStore.Inventory.Application.Abstractions.Common
{
    public interface IEventSourceRepository<TAggregate> where TAggregate : IAggregateRoot
    {
        Task<TAggregate?> Load(Guid aggregateId);

        Task Save(TAggregate aggregateRoot);
    }
}
