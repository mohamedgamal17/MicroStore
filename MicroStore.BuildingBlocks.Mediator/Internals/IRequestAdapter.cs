using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
namespace MicroStore.BuildingBlocks.Mediator.Internals
{
    public interface IRequestAdapter<TResponse> : MediatR.IRequest<TResponse>
    {
         IBaseRequest Request { get; }

         Type RequestType { get; }

         Type ResponseType { get; }
    }
}
