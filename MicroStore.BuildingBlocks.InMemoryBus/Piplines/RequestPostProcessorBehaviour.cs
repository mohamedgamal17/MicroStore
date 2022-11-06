using MicroStore.BuildingBlocks.InMemoryBus.Contracts;


namespace MicroStore.BuildingBlocks.InMemoryBus.Piplines
{
    public class RequestPostProcessorBehaviour<TRequest, TResponse> : RequestMiddleware<TRequest, TResponse>
     where TRequest : IRequest<TResponse>
    {


        private readonly IEnumerable<IRequestPostProcess<TRequest, TResponse>> _postProcessors;

        public RequestPostProcessorBehaviour(IEnumerable<IRequestPostProcess<TRequest, TResponse>> postProcessors)
        {
            _postProcessors = postProcessors;
        }

        public override async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var result = await next();

            foreach (var processor in _postProcessors)
            {
                await processor.Process(request, result, cancellationToken);
            }

            return result;
        }
    }
}
