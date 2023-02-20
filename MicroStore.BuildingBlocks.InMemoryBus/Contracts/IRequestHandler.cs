

using MicroStore.BuildingBlocks.Results;

namespace MicroStore.BuildingBlocks.InMemoryBus.Contracts
{
 
    public interface IRequestHandler<TRequest,TResponse>  where TRequest : IRequest<TResponse>
    {
        Task<ResponseResult<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
    }


    public interface IRequestHandlerV2<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }



}
