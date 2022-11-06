using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Mediator.Internals;

namespace MicroStore.BuildingBlocks.Mediator
{
    public class RequestAdapter<TResponse> : IRequestAdapter<TResponse>
    {
        public IRequest<TResponse> Request { get; }

        public Type RequestType { get; }


        public Type ResponseType { get; }


        public RequestAdapter(IRequest<TResponse> request)
        {
            Request = request;

            RequestType = request.GetType();

            ResponseType = typeof(TResponse);
        }
    }
}
