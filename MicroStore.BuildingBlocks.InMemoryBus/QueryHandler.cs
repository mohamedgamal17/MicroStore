using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.BuildingBlocks.InMemoryBus
{   
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse> 
        where TQuery : IQuery<TResponse>
    {

    }

    public abstract class QueryHandler<TQuery,TResponse> : RequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
    }
}
