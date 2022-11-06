using MicroStore.BuildingBlocks.InMemoryBus.Contracts;


namespace MicroStore.BuildingBlocks.InMemoryBus.Piplines
{

    public class RequestPreProcessorBehavior<TRequest, TResponse> : RequestMiddleware<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {

        private readonly IEnumerable<IRequestPreProcessor<TRequest>> _preProcessors;

        public RequestPreProcessorBehavior(IEnumerable<IRequestPreProcessor<TRequest>> preProcessors)
        {
            _preProcessors = preProcessors;
        }

        public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            foreach (var processor in _preProcessors)
            {
                await processor.Process(request, cancellationToken).ConfigureAwait(false);
            }

            return await next().ConfigureAwait(false);
        }
    }
}
