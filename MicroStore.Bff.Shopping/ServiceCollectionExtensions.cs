using MicroStore.Bff.Shopping.Config;
using MicroStore.Bff.Shopping;
using MicroStore.Bff.Shopping.Grpc;
using MicroStore.Bff.Shopping.Services.Geographic;
using MicroStore.Bff.Shopping.Filters;
using Microsoft.Extensions.DependencyInjection;
namespace MicroStore.Bff.Shopping
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBffShopping(this IServiceCollection services, IConfiguration config)
        {
            AddWebServices(services);

            RegisterGrpc(services, config);

            RegisterApplicationService(services);

            return services;
        }


        private static void AddWebServices(IServiceCollection services)
        {
            services.AddMvc(opt =>
            {
                opt.Filters.Add(typeof(ExceptionHandlerFilter));
            });

            services.AddControllers();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();
        }
        private static void RegisterGrpc(IServiceCollection services, IConfiguration config)
        {
            var grpcConfig = new GrpcConfiguration();

            config.Bind("Grpc",grpcConfig);

            services.AddSingleton(grpcConfig);

            services.AddGrpcClient<Grpc.Geographic.CountryService.CountryServiceClient>(opt =>
            {
                opt.Address = new Uri(grpcConfig.Geographic);
            });
            services.AddGrpcClient<Grpc.Geographic.StateProvinceService.StateProvinceServiceClient>(opt =>
            {
                opt.Address = new Uri(grpcConfig.Geographic);
            });
        }
        private static void RegisterApplicationService(IServiceCollection services)
        {
            services.AddTransient<CountryService>();
        }
    }
}
