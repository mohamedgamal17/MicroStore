using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;

namespace MicroStore.BuildingBlocks.InMemoryBus.Piplines
{

    public class RequestPreProcessorBehavior<TRequest> : RequestMiddleware<TRequest>
        where TRequest : IRequest
    {

        private readonly IEnumerable<IRequestPreProcessor<TRequest>> _preProcessors;

        public RequestPreProcessorBehavior(IEnumerable<IRequestPreProcessor<TRequest>> preProcessors)
        {
            _preProcessors = preProcessors;
        }

        public override async Task<ResponseResult> Handle(TRequest request, RequestHandlerDelegate next, CancellationToken cancellationToken)
        {
            foreach (var processor in _preProcessors)
            {
                await processor.Process(request, cancellationToken).ConfigureAwait(false);
            }

            return await next().ConfigureAwait(false);
        }
    }
}
