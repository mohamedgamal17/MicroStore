
namespace MicroStore.BuildingBlocks.InMemoryBus.Contracts
{
    public interface IBaseRequest
    {
    }

    public interface IRequest<TResponse> : IBaseRequest { }
}
