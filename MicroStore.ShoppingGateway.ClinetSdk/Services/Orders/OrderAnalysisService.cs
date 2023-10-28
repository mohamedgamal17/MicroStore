using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Orders
{
    public class OrderAnalysisService
    {
        const string BaseUrl = "/ordering/anaylsis/orders";

        private readonly MicroStoreClinet _microStoreClinet;

        public OrderAnalysisService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }

        public async Task<ForecastModel> Forecast(ForecastRequestOptions option , CancellationToken
             cancellationToken  =default)
        {
            var path = string.Format("{0}/{1}", BaseUrl, "forecast");

            return await _microStoreClinet.MakeRequest<ForecastModel>(path,HttpMethod.Post, option, cancellationToken);
        }

        public async Task<PagedList<OrderSalesReport>> GetOrderSalesReport(OrderSalesReportRequestOptions options, CancellationToken cancellationToken = default)
        {
            var path = string.Format("{0}/{1}", BaseUrl, "sales-report");
            return await _microStoreClinet.MakeRequest<PagedList<OrderSalesReport>>(path, HttpMethod.Get, options, cancellationToken);
        }


        public async Task<OrderSummaryReport> GetOrderSummaryReport(CancellationToken cancellationToken = default)
        {
            var path = string.Format("{0}/{1}", BaseUrl, "summary-report");

            return await _microStoreClinet.MakeRequest<OrderSummaryReport>(path, HttpMethod.Get, cancellationToken: cancellationToken);
        }
    }
}
