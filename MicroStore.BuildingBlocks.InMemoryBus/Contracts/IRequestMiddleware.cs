

namespace MicroStore.BuildingBlocks.InMemoryBus.Contracts
{
    public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();

    public interface IRequestMiddleware<in TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
    }
}
