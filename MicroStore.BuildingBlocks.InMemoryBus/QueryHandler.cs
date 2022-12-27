using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
namespace MicroStore.BuildingBlocks.InMemoryBus
{
    public abstract class QueryHandler<TQuery> : RequestHandler<TQuery, ResponseResult>
        where TQuery : IQuery
    {

    }

}
