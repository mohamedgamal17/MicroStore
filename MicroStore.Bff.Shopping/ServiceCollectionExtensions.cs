using MicroStore.Bff.Shopping.Config;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBffShopping(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            RegisterGrpc(services, config);

            return services;
        }


        private static void RegisterGrpc(IServiceCollection services, IConfiguration config)
        {
            var grpcConfig = config.GetValue<GrpcConfiguration>("Grpc");

            services.AddSingleton(grpcConfig);
        }
    }
}
