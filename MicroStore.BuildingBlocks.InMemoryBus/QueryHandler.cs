using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.BuildingBlocks.InMemoryBus
{   
    public abstract class QueryHandler<TQuery,TResponse> : RequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
    }
}
