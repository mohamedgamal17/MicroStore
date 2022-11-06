

namespace MicroStore.BuildingBlocks.InMemoryBus.Contracts
{
    public interface ILocalMessageBus
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}
