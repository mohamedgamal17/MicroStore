using Grpc.Core;
using Grpc.Core.Interceptors;
using System.Net;

namespace MicroStore.Bff.Shopping.Infrastructure
{
    public class GrpcClientTokenInterceptor : Interceptor
    {
        private readonly IServiceProvider _serviceProvider;
        public GrpcClientTokenInterceptor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            using var scope = _serviceProvider.CreateScope();
            var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
            var accessToken = tokenService.GetAccessToken().Result;

            if (!string.IsNullOrEmpty(accessToken))
            {
                var headers = new Metadata
                {
                    {"Authorization", $"Bearer {accessToken}"}
                };

                var callOption = context.Options.WithHeaders(headers);

                context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, callOption);         
            }

            return base.AsyncUnaryCall(request, context, continuation);
        }

    }
}
