using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Orders
{
    public class OrderAnalysisService : Service
    {
        const string BaseUrl = "/ordering/anaylsis/orders";

        public OrderAnalysisService(MicroStoreClinet microStoreClinet) : base(microStoreClinet)
        {
        }

        public async Task<ForecastModel> Forecast(ForecastRequestOptions option , RequestHeaderOptions requestHeaderOptions = null, CancellationToken
             cancellationToken  =default)
        {
            var path = string.Format("{0}/{1}", BaseUrl, "forecast");

            return await MakeRequestAsync<ForecastModel>(path,HttpMethod.Post, option, requestHeaderOptions, cancellationToken);
        }

        public async Task<PagedList<OrderSalesReport>> GetOrderSalesReport(OrderSalesReportRequestOptions options, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            var path = string.Format("{0}/{1}", BaseUrl, "sales-report");
            return await MakeRequestAsync<PagedList<OrderSalesReport>>(path, HttpMethod.Get, options, requestHeaderOptions, cancellationToken);
        }


        public async Task<OrderSummaryReport> GetOrderSummaryReport(RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            var path = string.Format("{0}/{1}", BaseUrl, "summary-report");

            return await MakeRequestAsync<OrderSummaryReport>(path, HttpMethod.Get, requestHeaderOptions : requestHeaderOptions, cancellationToken: cancellationToken);
        }
    }
}
