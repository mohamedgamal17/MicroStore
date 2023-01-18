using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using Volo.Abp.Uow;

namespace MicroStore.BuildingBlocks.InMemoryBus
{
    public abstract class RequestMiddleware<TRequest, TResponse> :  IRequestMiddleware<TRequest,TResponse>, IUnitOfWorkEnabled
        where TRequest : IRequest<TResponse>
    {
        public abstract Task<ResponseResult<TResponse>> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
    }
}
