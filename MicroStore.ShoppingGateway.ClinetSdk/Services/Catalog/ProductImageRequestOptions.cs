using MicroStore.ShoppingGateway.ClinetSdk.Common;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class ProductImageCreateOptions
    {
        public ProductImageModel Image { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class ProductImageUpdateOptions
    {
        public int DisplayOrder { get; set; }
    }
}
