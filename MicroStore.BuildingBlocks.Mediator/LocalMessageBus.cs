using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;

namespace MicroStore.BuildingBlocks.Mediator
{
    public class LocalMessageBus : ILocalMessageBus
    {
        private readonly MediatR.IMediator _mediator;

        public LocalMessageBus(MediatR.IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<ResponseResult> Send(IRequest request, CancellationToken cancellationToken = default)

        {
            return _mediator.Send(new RequestAdapter<ResponseResult>(request), cancellationToken);
        }
    }
}
