using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Inventory;
using MicroStore.ShoppingGateway.ClinetSdk.Interfaces;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Inventory
{
    public class InventoryItemService : Service ,
        IListableWithPaging<InventoryItem, PagingAndSortingRequestOptions>,
        IRetrievable<InventoryItem>,
        IUpdateable<InventoryItem, InventoryItemRequestOptions>
    {
        const string BASE_URL = "/inventory/products";

        private readonly MicroStoreClinet _microStoreClinet;

        public InventoryItemService(MicroStoreClinet microStoreClinet)
            : base(microStoreClinet)
        {

        }

        public async Task<InventoryItem> UpdateAsync(string id,InventoryItemRequestOptions options, RequestHeaderOptions requestHeaderOptions = null , CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<InventoryItem>(string.Format("{0}/{1}", BASE_URL, id), HttpMethod.Put, options,requestHeaderOptions ,cancellationToken: cancellationToken);
        }


        public async Task<PagedList<InventoryItem>> ListAsync(PagingAndSortingRequestOptions options = null, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<PagedList<InventoryItem>>(BASE_URL,HttpMethod.Get ,options, requestHeaderOptions,cancellationToken);
        }

        public async Task<InventoryItem> GetAsync(string itemId , RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken =default)
        {
            return await MakeRequestAsync<InventoryItem>(string.Format("{0}/{1}", BASE_URL, itemId),HttpMethod.Get,requestHeaderOptions: requestHeaderOptions, cancellationToken: cancellationToken);
        }

        public async Task<InventoryItem> GetBySkyAsync(string sku, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<InventoryItem>(string.Format("{0}/sku/{1}", BASE_URL, sku), HttpMethod.Get, requestHeaderOptions: requestHeaderOptions, cancellationToken: cancellationToken);
        }

    }
}
