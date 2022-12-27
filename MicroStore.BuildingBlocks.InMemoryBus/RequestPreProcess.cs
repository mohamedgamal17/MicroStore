using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.BuildingBlocks.InMemoryBus
{
    public abstract class RequestPreProcess<TRequest> : IRequestPreProcessor<TRequest> where TRequest : IRequest
    {
        public abstract Task Process(TRequest request, CancellationToken cancellationToken);

    }
}