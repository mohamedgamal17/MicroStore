using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
namespace MicroStore.BuildingBlocks.InMemoryBus
{

    public abstract class QueryHandler<TQuery, TResponse> : RequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {

    }

    public abstract class QueryHandler<TQuery> : RequestHandler<TQuery, ResponseResult>
        where TQuery : IQuery
    {

    }

}
