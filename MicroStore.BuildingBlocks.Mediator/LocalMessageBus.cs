using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.BuildingBlocks.Mediator
{
    public class LocalMessageBus : ILocalMessageBus
    {
        private readonly MediatR.IMediator _mediator;

        public LocalMessageBus(MediatR.IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)

        {
            return _mediator.Send(new RequestAdapter<TResponse>(request), cancellationToken);
        }
    }
}
