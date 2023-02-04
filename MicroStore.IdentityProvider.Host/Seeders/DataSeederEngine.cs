using Duende.IdentityServer.Services;
using Volo.Abp.DependencyInjection;
namespace MicroStore.IdentityProvider.Host.Seeders
{
    [ExposeServices(typeof(IDataSeederEngine))]
    public class DataSeederEngine : IDataSeederEngine , ISingletonDependency
    {
        private readonly ICancellationTokenProvider _cancellationTokenProvider;

        public DataSeederEngine(ICancellationTokenProvider cancellationTokenProvider)
        {
            _cancellationTokenProvider = cancellationTokenProvider;
        }

        public async Task TryToSeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();

            var context = new DataSeederContext(scope.ServiceProvider);

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(context, _cancellationTokenProvider.CancellationToken);
            }
        }
    }
}
