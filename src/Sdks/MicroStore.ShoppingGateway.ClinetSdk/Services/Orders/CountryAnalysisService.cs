using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Orders
{
    public class CountryAnalysisService : Service
    {
        const string BASE_URL = "/ordering/anaylsis/countries";

        public CountryAnalysisService(MicroStoreClinet microStoreClinet) : base(microStoreClinet)
        {
        }

        public async Task<ForecastModel> Forecast(string countryCode,ForecastRequestOptions option = null,RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}/{2}", BASE_URL, countryCode, "forecast");

            return await MakeRequestAsync<ForecastModel>(path, HttpMethod.Post, option,requestHeaderOptions ,cancellationToken);
        }

        public async Task<PagedList<CountrySalesSummary>> GetCountriesSalesSummary(PagingReqeustOptions options = null , RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}", BASE_URL, "sales-summary-report");

            return await MakeRequestAsync<PagedList<CountrySalesSummary>>(path,HttpMethod.Get, options,requestHeaderOptions : requestHeaderOptions  , cancellationToken: cancellationToken);
        }

        public async Task<PagedList<CountrySalesReport>> GetCountrySalesReport(string countryCode,CountrySalesReportRequestOptions options,RequestHeaderOptions requestHeaderOptions = null,CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}/{2}", BASE_URL, countryCode, "sales-report");

            return await MakeRequestAsync<PagedList<CountrySalesReport>>(path, HttpMethod.Get, options, requestHeaderOptions, cancellationToken);
        }
    }
}
