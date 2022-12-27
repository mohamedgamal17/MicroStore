using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;

namespace MicroStore.BuildingBlocks.InMemoryBus.Wrappers
{
    public abstract class RequestHandlerBase
    {
        public abstract Task<object?> Handle(object request, CancellationToken cancellationToken);
    }
    public abstract class RequestHandlerWrapper<TResponse> : RequestHandlerBase
    {
        public abstract Task<ResponseResult> Handle(IRequest request, CancellationToken cancellationToken);
    }


    public class RequestHandlerWrapperImpl<TRequest, TResponse> : RequestHandlerWrapper<TResponse>
        where TRequest : IRequest
    {

        private readonly IServiceProvider _serviceProvider;

        public RequestHandlerWrapperImpl(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override async Task<object?> Handle(object request, CancellationToken cancellationToken)
        {
            return await Handle((IRequest)request, cancellationToken);
        }

        public override Task<ResponseResult> Handle(IRequest request, CancellationToken cancellationToken)
        {


            Task<ResponseResult> handler() => _serviceProvider
                .GetRequiredService<IRequestHandler<TRequest>>()
                .Handle((TRequest)request, cancellationToken);


            return _serviceProvider.GetServices<IRequestMiddleware<TRequest>>()
                .Reverse()
                .Aggregate((RequestHandlerDelegate)handler, (next, pipeline) =>
                    () => pipeline.Handle((TRequest)request, next, cancellationToken))();
        }
    }
}
