using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using System.Threading.Tasks.Dataflow;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Orders
{
    public class ProductAnalysisService : Service
    {
        const string BaseUrl = "/ordering/anaylsis/products";

        private readonly ProductService _productService;
        public ProductAnalysisService(MicroStoreClinet microStoreClinet, ProductService productService) : base(microStoreClinet)
        {
            _productService = productService;
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

        public async Task<PagedList<BestSellerReportAggregate>> GetBestSellersReportAggregate(PagingAndSortingRequestOptions options, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {

            var response = await GetBestSellersReport( options,requestHeaderOptions, cancellationToken);

            var tasks = response.Items.Select(x => PrepareBestSellerReportAggregate(x, cancellationToken));

            var aggregates = await Task.WhenAll(tasks);

            var model = new PagedList<BestSellerReportAggregate>
            {
                Items = aggregates.ToList(),
                Skip = response.Skip,
                Lenght = response.Lenght,
                TotalCount = response.TotalCount
            };

            return model;
        }


        public async Task<PagedList<ProductSalesReport>> GetProductSalesReport( string productId, ProductSalesReportRequestOptions options, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}/{2}", BaseUrl, productId, "units-summary");

            return await MakeRequestAsync<PagedList<ProductSalesReport>>(path, HttpMethod.Get, options, requestHeaderOptions, cancellationToken);
        }

        private async Task<BestSellerReportAggregate> PrepareBestSellerReportAggregate(BestSellerReport bestSellerReport, CancellationToken cancellationToken = default)
        {
            var product=  await _productService.GetAsync(bestSellerReport.ProductId, cancellationToken: cancellationToken);

            var aggregate = new BestSellerReportAggregate
            {
                ProductId = bestSellerReport.ProductId,
                Amount =  bestSellerReport.Amount,
                Name = product.Name,
                Thumbnail = product.ProductImages.FirstOrDefault()?.Image,
                Quantity = bestSellerReport.Quantity,

            };


            return aggregate;
        }

        private class BestSellerReportAggregatePiplineInput
        {
            public PagingAndSortingRequestOptions RequestOptions { get; set; }

            public RequestHeaderOptions RequestHeaderOptions { get; set; }  
        }
    }

    
}
