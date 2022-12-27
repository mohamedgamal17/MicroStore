using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using System.Net;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Validation;
namespace MicroStore.BuildingBlocks.InMemoryBus
{
    [DisableValidation]
    public abstract class RequestHandler<TRequest> : IRequestHandler<TRequest>, IValidationEnabled
      where TRequest : IRequest
    {
        public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = null!;
        protected Type? ObjectMapperContext { get; set; }
        protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetService<IObjectMapper>(provider =>
            ObjectMapperContext == null
                ? provider.GetRequiredService<IObjectMapper>()
                : (IObjectMapper)provider.GetRequiredService(typeof(IObjectMapper<>).MakeGenericType(ObjectMapperContext)));


        public abstract Task<ResponseResult> Handle(TRequest request, CancellationToken cancellationToken);


        protected ResponseResult Success(HttpStatusCode statusCode)
        {
            return ResponseResult.Success((int)statusCode);
        }

        protected ResponseResult Success<T>(HttpStatusCode statusCode , T result)
        {
            return ResponseResult.Success<T>((int)statusCode, result);
        }

        protected ResponseResult Failure(HttpStatusCode stausCode , string errorMessage , string? details = null)
        {
            return ResponseResult.Failure((int)stausCode, new ErrorInfo
            {
                Message = errorMessage,
                Details = details
            });
        }

        protected ResponseResult Failure(HttpStatusCode stausCode , ErrorInfo errorInfo)
        {
            return ResponseResult.Failure((int)stausCode, errorInfo);
        }
    }
}
