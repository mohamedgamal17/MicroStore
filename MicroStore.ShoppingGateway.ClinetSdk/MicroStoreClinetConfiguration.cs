namespace MicroStore.ShoppingGateway.ClinetSdk
{
    public delegate Task<string> TokenHandler(IServiceProvider serviceProvider);

    public class MicroStoreClinetConfiguration
    {
        public TokenHandler TokenHandlerDeleagete { get; set; }  

    }
}
