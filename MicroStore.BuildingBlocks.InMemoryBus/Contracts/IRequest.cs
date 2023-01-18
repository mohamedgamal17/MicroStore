
using MicroStore.BuildingBlocks.Results;

namespace MicroStore.BuildingBlocks.InMemoryBus.Contracts
{

    public interface IBaseRequest { }


    public interface IRequest<TResponse>  : IBaseRequest{ }

}
