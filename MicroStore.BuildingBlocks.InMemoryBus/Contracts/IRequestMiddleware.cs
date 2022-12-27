

using MicroStore.BuildingBlocks.Results;

namespace MicroStore.BuildingBlocks.InMemoryBus.Contracts
{
    public delegate Task<ResponseResult> RequestHandlerDelegate();

    public interface IRequestMiddleware<in TRequest> where TRequest : IRequest

    {
        Task<ResponseResult> Handle(TRequest request, RequestHandlerDelegate next, CancellationToken cancellationToken);

    }
}
