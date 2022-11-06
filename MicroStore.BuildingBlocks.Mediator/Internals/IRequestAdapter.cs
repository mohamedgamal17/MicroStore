
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.BuildingBlocks.Mediator.Internals
{
    public interface IRequestAdapter<TResponse> : MediatR.IRequest<TResponse>
    {
        public IRequest<TResponse> Request { get; }

        public Type RequestType { get; }

        public Type ResponseType { get; }
    }
}
