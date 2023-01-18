using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Mediator.Internals;
using MicroStore.BuildingBlocks.Results;

namespace MicroStore.BuildingBlocks.Mediator
{
    public class RequestAdapter<TResponse> : IRequestAdapter<ResponseResult<TResponse>>
    {
        public IBaseRequest Request { get; }

        public Type RequestType { get; }


        public Type ResponseType { get; }


        public RequestAdapter(IBaseRequest request)
        {
            Request = request;

            RequestType = request.GetType();

            ResponseType = typeof(TResponse);
        }
    }
}
