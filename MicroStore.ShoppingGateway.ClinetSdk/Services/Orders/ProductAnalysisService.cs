using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Orders
{
    public class ProductAnalysisService
    {
        const string BaseUrl = "/ordering/anaylsis/products";

        private readonly MicroStoreClinet _microStoreClinet;
        public ProductAnalysisService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }

        public async Task<ForecastModel> Forecast(string productId , ForecastRequestOptions options, CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}/{2}", BaseUrl, productId, "forecast");

            return await _microStoreClinet.MakeRequest<ForecastModel>(path, HttpMethod.Post, options, cancellationToken);
        }

        public async Task<PagedList<BestSellerReport>> GetBestSellersReport(PagingAndSortingRequestOptions options, CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}", BaseUrl, "bestsellers");
            return await _microStoreClinet.MakeRequest<PagedList<BestSellerReport>>(path, HttpMethod.Get,options,cancellationToken);
        }

        public async Task<List<ProductSalesReport>> GetProductUnitSalesReport( string productId, ProductSalesUnitReportRequestOptions options, CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}/{2}", BaseUrl, productId, "units-summary");

            return await _microStoreClinet.MakeRequest<List<ProductSalesReport>>(path, HttpMethod.Get, options, cancellationToken);
        }
    }
}
