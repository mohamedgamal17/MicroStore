using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.InMemoryBus.Wrappers;
using MicroStore.BuildingBlocks.Mediator.Internals;
using MicroStore.BuildingBlocks.Results;

namespace MicroStore.BuildingBlocks.Mediator
{
    public class ReqeustHandlerAdapter<TRequest> : MediatR.IRequestHandler<TRequest, ResponseResult>
    where TRequest : IRequestAdapter
    {

        private readonly IServiceProvider _serviceProvider;

        public ReqeustHandlerAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<ResponseResult> Handle(TRequest request, CancellationToken cancellationToken)
        {

            var requestHandlerWrapperType = typeof(RequestHandlerWrapperImpl<,>)
             .MakeGenericType(request.RequestType, request.ResponseType);

            var handler = (RequestHandlerBase)_serviceProvider.GetRequiredService(requestHandlerWrapperType);

            return (ResponseResult)await handler.Handle(request.Request, cancellationToken);

        }
    }
}
