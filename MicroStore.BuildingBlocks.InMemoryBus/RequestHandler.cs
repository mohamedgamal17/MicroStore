using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using System.Net;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AutoMapper;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Validation;
namespace MicroStore.BuildingBlocks.InMemoryBus
{
    public abstract class RequestHandler
    {
        public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = null!;
        protected IMapperAccessor MapperAccessor => LazyServiceProvider.LazyGetRequiredService<IMapperAccessor>();
        protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetService<IObjectMapper>(provider => provider.GetRequiredService<IObjectMapper>());


        protected ResponseResult Success(HttpStatusCode statusCode)
        {
            return ResponseResult.Success((int)statusCode);
        }

        protected ResponseResult<T> Success<T>(HttpStatusCode statusCode, T result)
        {
            return ResponseResult.Success((int)statusCode, result);
        }

        protected ResponseResult<ListResultDto<T>> Success<T>(HttpStatusCode statusCode, List<T> result)
        {
            return ResponseResult.Success((int)statusCode, new ListResultDto<T>(result));
        }

        protected ResponseResult Failure(HttpStatusCode statusCode, string errorMessage, string? details = null)
        {
            return ResponseResult.Failure((int)statusCode, new ErrorInfo
            {
                Message = errorMessage,
                Details = details ?? string.Empty
            });
        }
        protected ResponseResult Failure(HttpStatusCode statusCode, ErrorInfo errorInfo)
        {
            return ResponseResult.Failure((int)statusCode, errorInfo);
        }

        protected ResponseResult<T> Failure<T>(HttpStatusCode statusCode, string errorMessage, string? details = null)
        {
            return ResponseResult.Failure<T>((int)statusCode, new ErrorInfo
            {
                Message = errorMessage,
                Details = details
            });
        }
        protected ResponseResult<T> Failure<T>(HttpStatusCode stausCode, ErrorInfo errorInfo)
        {
            return ResponseResult.Failure<T>((int)stausCode, errorInfo);
        }



    }

    [DisableValidation]
    public abstract class RequestHandler<TRequest,TResponse> : IRequestHandler<TRequest, TResponse>
      where TRequest : IRequest<TResponse>
    {
        public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = null!;
        protected Type? ObjectMapperContext { get; set; }
        protected IMapperAccessor MapperAccessor => LazyServiceProvider.LazyGetRequiredService<IMapperAccessor>();
        protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetService<IObjectMapper>(provider =>
            ObjectMapperContext == null
                ? provider.GetRequiredService<IObjectMapper>()
                : (IObjectMapper)provider.GetRequiredService(typeof(IObjectMapper<>).MakeGenericType(ObjectMapperContext)));


        public abstract Task<ResponseResult<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);


        protected ResponseResult Success(HttpStatusCode statusCode)
        {
            return ResponseResult.Success((int)statusCode);
        }

        protected ResponseResult<TResponse> Success(HttpStatusCode statusCode, TResponse result)
        {
            return ResponseResult.Success<TResponse>((int)statusCode, result);
        }

        protected ResponseResult<T> Success<T>(HttpStatusCode statusCode, T result)
        {
            return ResponseResult.Success((int)statusCode, result);
        }

        protected ResponseResult<ListResultDto<T>> Success<T>(HttpStatusCode statusCode, List<T> result)
        {
            return ResponseResult.Success((int)statusCode, new ListResultDto<T>(result));
        }

        protected ResponseResult<TResponse> Failure(HttpStatusCode statusCode, string errorMessage, string? details = null)
        {
            return ResponseResult.Failure<TResponse>((int)statusCode, new ErrorInfo
            {
                Message = errorMessage,
                Details = details
            });
        }


        protected ResponseResult<T> Failure<T>(HttpStatusCode statusCode, string errorMessage, string? details = null)
        {
            return ResponseResult.Failure<T>((int)statusCode, new ErrorInfo
            {
                Message = errorMessage,
                Details = details
            });
        }



        protected ResponseResult<TResponse> Failure(HttpStatusCode stausCode, ErrorInfo errorInfo)
        {
            return ResponseResult.Failure<TResponse>((int)stausCode, errorInfo);
        }

        protected ResponseResult<T> Failure<T>(HttpStatusCode stausCode, ErrorInfo errorInfo)
        {
            return ResponseResult.Failure<T>((int)stausCode, errorInfo);
        }

    }
}
