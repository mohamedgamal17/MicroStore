

using MicroStore.BuildingBlocks.Results;

namespace MicroStore.BuildingBlocks.InMemoryBus.Contracts
{
 
    public interface IRequestHandler<TRequest>  where TRequest : IRequest
    {
        Task<ResponseResult> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
