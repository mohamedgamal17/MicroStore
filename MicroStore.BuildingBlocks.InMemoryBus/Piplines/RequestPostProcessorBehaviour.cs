using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;

namespace MicroStore.BuildingBlocks.InMemoryBus.Piplines
{
    public class RequestPostProcessorBehaviour<TRequest> : RequestMiddleware<TRequest>
     where TRequest : IRequest
    {


        private readonly IEnumerable<IRequestPostProcess<TRequest>> _postProcessors;

        public RequestPostProcessorBehaviour(IEnumerable<IRequestPostProcess<TRequest>> postProcessors)
        {
            _postProcessors = postProcessors;
        }

        public override async Task<ResponseResult> Handle(TRequest request, RequestHandlerDelegate next, CancellationToken cancellationToken)
        {
            var result = await next().ConfigureAwait(false);

            foreach (var processor in _postProcessors)
            {
                await processor.Process(request, result, cancellationToken);
            }

            return result;
        }
    }
}
