using MicroStore.BuildingBlocks.InMemoryBus.Contracts;


namespace MicroStore.BuildingBlocks.InMemoryBus
{
    public abstract class RequestPostProcess<TReqeust, TResponse> : IRequestPostProcess<TReqeust, TResponse>
       where TReqeust : IRequest<TResponse>
    {
        public abstract Task Process(TReqeust request, TResponse response, CancellationToken cancellationToken);

    }
}
