using MicroStore.ShoppingGateway.ClinetSdk;
using System.Reflection;
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddMicroStoreClinet(this IServiceCollection services, Action<MicroStoreClinetConfiguration> action = null)
        {
            var config = new MicroStoreClinetConfiguration();

            if(action != null)
            {
                action(config);
            }
  
            services.AddSingleton(config);

            services.AddTransient<MicroStoreClinet>();

            ConventionalRegistarServices(services);

            return services;
        }

        private static void ConventionalRegistarServices(IServiceCollection services)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes()
                                .Where(x => x.Name.EndsWith("Service"))
                                .Where(x => x.IsClass)
                                .ToList();


            types.ForEach((type) => services.AddTransient(type));

        }
    }
}
