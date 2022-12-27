
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;

namespace MicroStore.BuildingBlocks.Mediator.Internals
{
    public interface IRequestAdapter : MediatR.IRequest<ResponseResult>
    {
         IRequest Request { get; }

         Type RequestType { get; }

         Type ResponseType { get; }
    }
}
