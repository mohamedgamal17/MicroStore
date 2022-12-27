using MicroStore.BuildingBlocks.Results;
namespace MicroStore.BuildingBlocks.InMemoryBus.Contracts
{
    public interface IRequestPostProcess<in TRequest> where TRequest : IRequest
    {
        Task Process(TRequest request, ResponseResult response, CancellationToken cancellationToken);

    }
}
