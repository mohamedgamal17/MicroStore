using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Mediator.Internals;
using MicroStore.BuildingBlocks.Results;

namespace MicroStore.BuildingBlocks.Mediator
{
    public class RequestAdapter : IRequestAdapter
    {
        public IRequest Request { get; }

        public Type RequestType { get; }


        public Type ResponseType { get; }


        public RequestAdapter(IRequest request)
        {
            Request = request;

            RequestType = request.GetType();

            ResponseType = typeof(ResponseResult);
        }
    }
}
