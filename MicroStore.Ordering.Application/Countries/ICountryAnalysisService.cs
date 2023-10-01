using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.Ordering.Application.Countries
{
    public interface ICountryAnalysisService : IApplicationService
    {
        Task<Result<ForecastDto>> ForecastSales(string countryCode,ForecastModel model ,CancellationToken cancellationToken = default);

        Task<Result<AggregatedMonthlyReportDto>> GetSalesMonthlyReport(string countryCode, CancellationToken cancellationToken= default);

        Task<Result<AggregateDailyReportDto>> GetSalesDailyReport(string countryCode, DailyReportModel model, CancellationToken cancellationToken = default);
      
    }
}
