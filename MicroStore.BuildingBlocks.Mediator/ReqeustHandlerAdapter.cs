using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.InMemoryBus.Wrappers;
using MicroStore.BuildingBlocks.Mediator.Internals;
using MicroStore.BuildingBlocks.Results;

namespace MicroStore.BuildingBlocks.Mediator
{
    public class ReqeustHandlerAdapter<TRequest,TResponse> : MediatR.IRequestHandler<TRequest, TResponse>
    where TRequest : IRequestAdapter<TResponse>
    {

        private readonly IServiceProvider _serviceProvider;

        public ReqeustHandlerAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {

            var requestHandlerWrapperType = typeof(RequestHandlerWrapperImpl<,>)
             .MakeGenericType(request.RequestType, request.ResponseType);

            var handler = (RequestHandlerBase)_serviceProvider.GetRequiredService(requestHandlerWrapperType);

            return (TResponse) await handler.Handle(request.Request, cancellationToken).ConfigureAwait(false);

        }
    }
}
