using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;


namespace MicroStore.BuildingBlocks.InMemoryBus.Wrappers
{
    public abstract class RequestHandlerBase
    {
        public abstract Task<object?> Handle(object request, CancellationToken cancellationToken);
    }
    public abstract class RequestHandlerWrapper<TResponse> : RequestHandlerBase
    {
        public abstract Task<TResponse> Handle(IRequest<TResponse> request, CancellationToken cancellationToken);
    }


    public class RequestHandlerWrapperImpl<TRequest, TResponse> : RequestHandlerWrapper<TResponse>
        where TRequest : IRequest<TResponse>
    {

        private readonly IServiceProvider _serviceProvider;

        public RequestHandlerWrapperImpl(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override async Task<object?> Handle(object request, CancellationToken cancellationToken)
        {
            return await Handle((IRequest<TResponse>)request, cancellationToken);
        }

        public override Task<TResponse> Handle(IRequest<TResponse> request, CancellationToken cancellationToken)
        {


            Task<TResponse> handler() => _serviceProvider
                .GetRequiredService<IRequestHandler<TRequest, TResponse>>()
                .Handle((TRequest)request, cancellationToken);


            return _serviceProvider.GetServices<IRequestMiddleware<TRequest, TResponse>>()
                .Reverse()
                .Aggregate((RequestHandlerDelegate<TResponse>)handler, (next, pipeline) =>
                    () => pipeline.Handle((TRequest)request, next, cancellationToken))();
        }
    }
}
