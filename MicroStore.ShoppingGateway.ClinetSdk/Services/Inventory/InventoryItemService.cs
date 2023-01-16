using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Inventory;
using MicroStore.ShoppingGateway.ClinetSdk.Extensions;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Inventory
{
    public class InventoryItemService
    {
        const string BaseUrl = "Inventory/products";

        private readonly MicroStoreClinet _microStoreClinet;

        public InventoryItemService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }

        public Task<HttpResponseResult<InventoryItem>> AdjustQuantityAsync(string sku,InventoryItemAdjustQuantityRequestOptions options , CancellationToken cancellationToken)
        {
            return _microStoreClinet.MakeRequest<InventoryItemAdjustQuantityRequestOptions, InventoryItem>(options, string.Format("{0}/adjustquantity/{1}", BaseUrl, sku), HttpMethod.Post, cancellationToken);
        }


        public Task<HttpResponseResult<PagedList<InventoryItem>>> ListAsync(PagingAndSortingRequestOptions options , CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeGetRequest<PagedList<InventoryItem>>(BaseUrl, options.ConvertToDictionary(), cancellationToken);
        }

        public Task<HttpResponseResult<InventoryItem>> RetrieveAsync(Guid itemId , CancellationToken cancellationToken)
        {
            return _microStoreClinet.MakeGetRequest<InventoryItem>(string.Format("{0}/{1}", BaseUrl, itemId), cancellationToken: cancellationToken);
        }

        public Task<HttpResponseResult<InventoryItem>> RetrieveBySkyAsync(string sku, CancellationToken cancellationToken)
        {
            return _microStoreClinet.MakeGetRequest<InventoryItem>(string.Format("{0}/sku/{1}", BaseUrl, sku), cancellationToken: cancellationToken);
        }

        public Task<HttpResponseResult<InventoryItem>> RetrieveByExternalProductId(string externalProductId, CancellationToken cancellationToken)
        {
            return _microStoreClinet.MakeGetRequest<InventoryItem>(string.Format("{0}/external_product_id/{1}", BaseUrl, externalProductId), cancellationToken: cancellationToken);
        }


    }
}
