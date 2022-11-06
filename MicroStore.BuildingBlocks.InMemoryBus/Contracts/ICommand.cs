

namespace MicroStore.BuildingBlocks.InMemoryBus.Contracts
{
    public interface ICommand<TResponse> : IRequest<TResponse>
    {

    }

    public interface ICommand : ICommand<Unit>
    {

    }
}
