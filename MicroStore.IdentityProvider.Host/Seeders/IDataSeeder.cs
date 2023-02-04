namespace MicroStore.IdentityProvider.Host.Seeders
{
    public interface IDataSeederEngine
    {
        Task TryToSeedAsync(IServiceProvider serviceProvider);
    }

    public interface IDataSeeder
    {
        Task SeedAsync(DataSeederContext context, CancellationToken cancellationToken = default);
    }



    public class DataSeederContext
    {
        public IServiceProvider ServiceProvider { get; set; }

        public DataSeederContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}
