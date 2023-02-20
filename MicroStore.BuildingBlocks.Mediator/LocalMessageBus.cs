using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using Volo.Abp.Application.Services;

namespace MicroStore.BuildingBlocks.Mediator
{
    public class LocalMessageBus : ILocalMessageBus
    {
        private readonly MediatR.IMediator _mediator;

        public LocalMessageBus(MediatR.IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<ResponseResult<TResponse>> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)

        {
            
            return _mediator.Send(new RequestAdapter<TResponse>(request), cancellationToken);
        }

        public Task<TResponse> SendV2<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
