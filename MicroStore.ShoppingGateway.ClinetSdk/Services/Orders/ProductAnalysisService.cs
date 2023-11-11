using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Orders
{
    public class ProductAnalysisService : Service
    {
        const string BaseUrl = "/ordering/anaylsis/products";

        public ProductAnalysisService(MicroStoreClinet microStoreClinet) : base(microStoreClinet)
        {

        }

        public async Task<ForecastModel> Forecast(string productId , ForecastRequestOptions options, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}/{2}", BaseUrl, productId, "forecast");

            return await MakeRequestAsync<ForecastModel>(path, HttpMethod.Post, options, requestHeaderOptions, cancellationToken);
        }

        public async Task<PagedList<BestSellerReport>> GetBestSellersReport(PagingAndSortingRequestOptions options, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}", BaseUrl, "bestsellers");
            return await MakeRequestAsync<PagedList<BestSellerReport>>(path, HttpMethod.Get,options,requestHeaderOptions, cancellationToken);
        }

        public async Task<PagedList<ProductSalesReport>> GetProductSalesReport( string productId, ProductSalesReportRequestOptions options, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}/{2}", BaseUrl, productId, "units-summary");

            return await MakeRequestAsync<PagedList<ProductSalesReport>>(path, HttpMethod.Get, options, requestHeaderOptions, cancellationToken);
        }
    }
}
