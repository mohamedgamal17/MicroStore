using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using Volo.Abp.Application.Services;
namespace MicroStore.Ordering.Application.Orders
{
    public interface IOrderAnalysisService : IApplicationService
    {
        Task<Result<ForecastDto>> ForecastSales(ForecastModel model, CancellationToken cancellationToken = default);

        Task<Result<List<OrderSummaryReport>>> GetOrdersSummaryReport(OrderSummaryReportModel model, CancellationToken cancellationToken = default);
    }
}
