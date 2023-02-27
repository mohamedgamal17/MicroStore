using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Inventory;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Inventory
{
    public class InventoryItemService
    {
        const string BaseUrl = "/inventory/products";

        private readonly MicroStoreClinet _microStoreClinet;

        public InventoryItemService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }

        public async Task<InventoryItem> AdjustQuantityAsync(string sku,InventoryItemAdjustQuantityRequestOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<InventoryItem>( string.Format("{0}/adjustquantity/{1}", BaseUrl, sku), HttpMethod.Post, options, cancellationToken);
        }


        public Task<PagedList<InventoryItem>> ListAsync(PagingAndSortingRequestOptions options , CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<PagedList<InventoryItem>>(BaseUrl,HttpMethod.Get ,options, cancellationToken);
        }

        public async Task<InventoryItem> RetrieveAsync(Guid itemId , CancellationToken cancellationToken)
        {
            return await _microStoreClinet.MakeRequest<InventoryItem>(string.Format("{0}/{1}", BaseUrl, itemId),HttpMethod.Get, cancellationToken: cancellationToken);
        }

        public async Task<InventoryItem> RetrieveBySkyAsync(string sku, CancellationToken cancellationToken)
        {
            return await _microStoreClinet.MakeRequest<InventoryItem>(string.Format("{0}/sku/{1}", BaseUrl, sku), HttpMethod.Get,  cancellationToken);
        }

        public Task<InventoryItem> RetrieveByExternalProductId(string externalProductId, CancellationToken cancellationToken)
        {
            return _microStoreClinet.MakeRequest<InventoryItem>(string.Format("{0}/external_product_id/{1}", BaseUrl, externalProductId),HttpMethod.Get , cancellationToken);
        }


    }
}
