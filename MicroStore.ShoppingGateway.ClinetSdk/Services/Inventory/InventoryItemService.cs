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

        public async Task<InventoryItem> UpdateAsync(string sku,InventoryItemRequestOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<InventoryItem>( string.Format("{0}/{1}", BaseUrl, sku), HttpMethod.Post, options, cancellationToken);
        }


        public Task<PagedList<InventoryItem>> ListAsync(PagingAndSortingRequestOptions options , CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<PagedList<InventoryItem>>(BaseUrl,HttpMethod.Get ,options, cancellationToken);
        }

        public async Task<InventoryItem> GetAsync(string itemId , CancellationToken cancellationToken =default)
        {
            return await _microStoreClinet.MakeRequest<InventoryItem>(string.Format("{0}/{1}", BaseUrl, itemId),HttpMethod.Get, cancellationToken: cancellationToken);
        }

        public async Task<InventoryItem> GetBySkyAsync(string sku, CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<InventoryItem>(string.Format("{0}/sku/{1}", BaseUrl, sku), HttpMethod.Get,  cancellationToken);
        }



    }
}
