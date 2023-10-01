using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.Ordering.Application.Orders
{
    public interface IOrderAnalysisService : IApplicationService
    {
        Task<Result<ForecastDto>> ForecastSales(ForecastModel model, CancellationToken cancellationToken = default);

        Task<Result<AggregatedMonthlyReportDto>> GetSalesMonthlyReport(CancellationToken cancellationToken = default);

        Task<Result<AggregateDailyReportDto>> GetSalesDailyReport(DailyReportModel model, CancellationToken cancellationToken = default);
    }
}
