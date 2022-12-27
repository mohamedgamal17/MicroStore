

using MicroStore.BuildingBlocks.Results;

namespace MicroStore.BuildingBlocks.InMemoryBus.Contracts
{
    public interface ILocalMessageBus
    {
        Task<ResponseResult> Send(IRequest request, CancellationToken cancellationToken = default);
    }
}
