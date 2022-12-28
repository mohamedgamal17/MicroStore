using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using Volo.Abp.Uow;

namespace MicroStore.BuildingBlocks.InMemoryBus
{
    public abstract class RequestMiddleware<TRequest> :  IRequestMiddleware<TRequest>, IUnitOfWorkEnabled
        where TRequest : IRequest
    {
        public abstract Task<ResponseResult> Handle(TRequest request, RequestHandlerDelegate next, CancellationToken cancellationToken);
    }
}
