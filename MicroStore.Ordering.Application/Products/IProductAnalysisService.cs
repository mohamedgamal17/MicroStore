using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.Ordering.Application.Products
{
    public interface IProductAnalysisService : IApplicationService
    {
        Task<Result<ForecastDto>> ForecastPrdocut(string productId,ForecastModel model,CancellationToken cancellationToken = default);

        Task<Result<AggregatedMonthlySalesReportDto>> GetProductMonthlyReport(string productId, CancellationToken cancellationToken = default);

        Task<Result<AggregateDailySalesReportDto>> GetProductDailySalesReport(string productId, DailySalesReportModel model ,CancellationToken cancellationToken = default);

    }
}
