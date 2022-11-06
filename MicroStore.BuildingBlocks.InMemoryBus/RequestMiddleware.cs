using MicroStore.BuildingBlocks.InMemoryBus.Contracts;


namespace MicroStore.BuildingBlocks.InMemoryBus
{
    public abstract class RequestMiddleware<TRequest, TResponse> : IRequestMiddleware<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public abstract Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);

    }
}
