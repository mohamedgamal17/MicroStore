using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Orders
{
    public class CountryAnalysisService
    {
        const string BASE_URL = "/ordering/anaylsis/countries";

        private readonly MicroStoreClinet _microStoreClient;
        public CountryAnalysisService(MicroStoreClinet microStoreClient)
        {
            _microStoreClient = microStoreClient;
        }

        public async Task<ForecastModel> Forecast(string countryCode,ForecastRequestOptions option, CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}/{2}", BASE_URL, countryCode, "forecast");

            return await _microStoreClient.MakeRequest<ForecastModel>(path, HttpMethod.Post, option, cancellationToken);
        }

        public async Task<PagedList<CountrySalesSummary>> GetCountriesSalesSummary(PagingReqeustOptions options , CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}", BASE_URL, "sales-summary-report");

            return await _microStoreClient.MakeRequest<PagedList<CountrySalesSummary>>(path,HttpMethod.Get, options, cancellationToken);
        }

        public async Task<PagedList<CountrySalesReport>> GetCountrySalesReport(string countryCode,CountrySalesReportRequestOptions options, CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}/{2}", BASE_URL, countryCode, "sales-report");

            return await _microStoreClient.MakeRequest<PagedList<CountrySalesReport>>(path, HttpMethod.Get, options, cancellationToken);
        }
    }
}
