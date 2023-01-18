using MicroStore.BuildingBlocks.Results;
namespace MicroStore.BuildingBlocks.InMemoryBus.Contracts
{
    public interface ILocalMessageBus
    {
        Task<ResponseResult<TResponse>> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}
