using Duende.IdentityServer.EntityFramework;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IdentityServerExtensions
    {

        public static IServiceCollection AddConfigurationStore( this IServiceCollection services,Action<ConfigurationStoreOptions> storeOptionsAction = null)
        {
            var storeOptions = new ConfigurationStoreOptions();

            storeOptionsAction?.Invoke(storeOptions);

            return services.AddSingleton(storeOptions);
        }

        public static  IServiceCollection AddOperationalStore(this IServiceCollection services, Action<OperationalStoreOptions> storeOptionsAction = null)
        {
            var storeOptions = new OperationalStoreOptions();
            storeOptionsAction?.Invoke(storeOptions);

            services.AddTransient<TokenCleanupService>();
            return services.AddSingleton(storeOptions);
        }
    }
}
