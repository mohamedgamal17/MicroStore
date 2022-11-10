
using MicroStore.Inventory.Application.Abstractions.Common;

namespace MicroStore.Inventory.Application.Abstractions
{
    public interface IEventSourceRepository<TAggregate> where TAggregate : class, IAggregateRoot
    {
        Task Load(TAggregate aggregateRoot);

        Task Save(TAggregate aggregateRoot);

    }
}
