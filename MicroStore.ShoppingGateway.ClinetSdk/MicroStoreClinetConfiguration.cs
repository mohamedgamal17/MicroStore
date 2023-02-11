namespace MicroStore.ShoppingGateway.ClinetSdk
{
    public delegate Task<string> TokenHandler(IServiceProvider serviceProvider);

    public class MicroStoreClinetConfiguration
    {
        public TokenHandler TokenHandlerDeleagete { get; set; }
        public string BaseUrl { get; set; } = "https://localhost:7062/mvcgateway";
    }
}
