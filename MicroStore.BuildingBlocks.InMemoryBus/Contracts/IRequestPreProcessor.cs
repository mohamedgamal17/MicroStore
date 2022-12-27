

namespace MicroStore.BuildingBlocks.InMemoryBus.Contracts
{
    public interface IRequestPreProcessor<in TRequest> where TRequest : IRequest
    {
        Task Process(TRequest request, CancellationToken cancellationToken);
    }
}
