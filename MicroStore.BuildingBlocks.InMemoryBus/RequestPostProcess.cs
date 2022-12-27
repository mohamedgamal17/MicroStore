using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;

namespace MicroStore.BuildingBlocks.InMemoryBus
{
    public abstract class RequestPostProcess<TReqeust, TResponse> : IRequestPostProcess<TReqeust>
       where TReqeust : IRequest
    {
        public abstract Task Process(TReqeust request, ResponseResult response, CancellationToken cancellationToken);

    }
}
