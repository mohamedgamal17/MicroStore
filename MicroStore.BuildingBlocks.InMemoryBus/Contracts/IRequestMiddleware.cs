

using MicroStore.BuildingBlocks.Results;

namespace MicroStore.BuildingBlocks.InMemoryBus.Contracts
{
    public delegate Task<ResponseResult<TResponse>> RequestHandlerDelegate<TResponse>();

    public interface IRequestMiddleware<in TRequest,TResponse> where TRequest : IRequest<TResponse>

    {
        Task<ResponseResult<TResponse>> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);

    }
}
