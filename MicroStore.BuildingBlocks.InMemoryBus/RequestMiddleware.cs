using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;

namespace MicroStore.BuildingBlocks.InMemoryBus
{
    public abstract class RequestMiddleware<TRequest> :  IRequestMiddleware<TRequest>
        where TRequest : IRequest
    {
        public abstract Task<ResponseResult> Handle(TRequest request, RequestHandlerDelegate next, CancellationToken cancellationToken);
    }
}
