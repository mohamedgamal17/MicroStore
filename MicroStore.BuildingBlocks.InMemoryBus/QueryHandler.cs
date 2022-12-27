using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using Volo.Abp.Validation;

namespace MicroStore.BuildingBlocks.InMemoryBus
{
   
    public abstract class QueryHandler<TQuery> : RequestHandler<TQuery>
        where TQuery : IQuery
    {

    }

}
