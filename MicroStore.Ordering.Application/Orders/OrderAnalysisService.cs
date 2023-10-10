using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Common;
using MicroStore.Ordering.Application.Domain;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using Volo.Abp;
namespace MicroStore.Ordering.Application.Orders
{
    public class OrderAnalysisService : OrderApplicationService , IOrderAnalysisService
    {
        private readonly IOrderDbContext _orderDbContext;

        public OrderAnalysisService(IOrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task<Result<ForecastDto>> ForecastSales(ForecastModel model, CancellationToken cancellationToken = default)
        {
            bool canForecastSales =  await CheckIfSalesCanMonthlyForecasted(cancellationToken);

            if (!canForecastSales)
            {
                return new Result<ForecastDto>(new UserFriendlyException("Sales cannot be forecasted yet.It must have  at least 12 months in series"));
            }

            var salesReport = await PrepareSalesMonthlyReport(includeCurrentMonth: false, cancellationToken);


            var forcast = PrepareForcastedModel(model, salesReport);

            return forcast;
        }


        public async Task<Result<List<OrderSalesReportDto>>> GetOrdersSalesReport(OrderSalesReportModel model, CancellationToken cancellationToken = default)
        {
            var query = _orderDbContext.Query<OrderSalesReport>()
                .AsNoTracking();


            if (!string.IsNullOrEmpty(model.Status))
            {
                query = query.Where(x => x.CurrentState.ToLower() == model.Status.ToLower());
            }
            else
            {
               query =  query.Where(x => x.CurrentState == OrderStatusConst.Completed);
            }
            DateTime endDate = model.EndDate ?? DateTime.Now;

            DateTime startDate = model.StartDate ?? endDate.AddDays(-17);


            query = query.Where(x => x.Date >= startDate && x.Date <= endDate);

            var projection = model.Period switch
            {
                ReportPeriod.Daily => query.Select(x => new OrderSalesReportDto
                {
                    TotalOrders = x.TotalOrders,
                    TotalShippingPrice = x.TotalShippingPrice,
                    TotalTaxPrice = x.TotalTaxPrice,
                    TotalPrice = x.TotalPrice,
                    Date = x.Date
                }),

                ReportPeriod.Monthly => query.GroupBy(x => new
                {
                    x.Date.Year,
                    x.Date.Month
                }).Select(x => new OrderSalesReportDto
                {
                    Date = x.Max(x=> x.Date),
                    TotalOrders = x.Sum(x => x.TotalOrders),
                    TotalShippingPrice = x.Sum(x => x.TotalShippingPrice),
                    TotalTaxPrice = x.Sum(x => x.TotalTaxPrice),
                    TotalPrice = x.Sum(x => x.TotalPrice),
                }),
                _ => query.GroupBy(x=> new { x.Date.Year })
                .Select(x=> new OrderSalesReportDto
                {
                    Date = x.Max(x => x.Date),
                    TotalOrders = x.Sum(x => x.TotalOrders),
                    TotalShippingPrice = x.Sum(x => x.TotalShippingPrice),
                    TotalTaxPrice = x.Sum(x => x.TotalTaxPrice),
                    TotalPrice = x.Sum(x => x.TotalPrice),
                })
            };

            return await projection.OrderBy(x=> x.Date).ToListAsync(cancellationToken);

        }

        private async Task<List<OrderSalesReportDto>> PrepareSalesMonthlyReport(bool includeCurrentMonth = true ,CancellationToken cancellationToken = default)
        {
            var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var startDate = endDate.AddYears(-3);


            var query = _orderDbContext.Query<OrderSalesReport>()
                .AsNoTracking()
                .Where(x => x.CurrentState == OrderStatusConst.Completed)
                .Where(x => x.Date >= startDate && x.Date <= endDate);

            var projection = from order in query
                             group order by new
                             {
                                 order.Date.Year,
                                 order.Date.Month,
                             } into grouped
                             select new OrderSalesReportDto
                             {
                             
                                 TotalOrders = grouped.Sum(x => x.TotalOrders),
                                 TotalShippingPrice = grouped.Sum(x => x.TotalShippingPrice),
                                 TotalTaxPrice = grouped.Sum(x => x.TotalTaxPrice),
                                 TotalPrice = grouped.Sum(x => x.TotalPrice),
                                 Date = grouped.Max(x => x.Date),
                             };

            return await projection.ToListAsync(cancellationToken);
        }


        private async Task<bool> CheckIfSalesCanMonthlyForecasted(CancellationToken cancellationToken = default)
        {

            var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var startDate = endDate.AddYears(-3);

            var count = await _orderDbContext.Query<OrderSalesReport>()
               .Where(x => x.CurrentState == OrderStatusConst.Completed)
               .Where(x => x.Date < endDate && x.Date >= startDate)
               .GroupBy(x => new { x.Date.Year, x.Date.Month })
               .CountAsync();

            return count >= 36;
        }

        private ForecastDto PrepareForcastedModel(ForecastModel model, List<OrderSalesReportDto> reports)
        {
            var mlContext = new MLContext();

            var dataView = mlContext.Data.LoadFromEnumerable(reports);


            var pipline = mlContext.Transforms.Conversion.ConvertType(inputColumnName: nameof(OrderSalesReportDto.TotalPrice), outputColumnName: nameof(OrderSalesReportDto.TotalPrice), outputKind: Microsoft.ML.Data.DataKind.Single)
                .Append(
                mlContext.Forecasting.ForecastBySsa(                    
                    inputColumnName: nameof(OrderSalesReportDto.TotalPrice),
                    outputColumnName: nameof(ForecastDto.ForecastedValues),
                    windowSize: 12,
                    seriesLength: reports.Count,
                    trainSize: reports.Count,
                    horizon: model.Horizon,
                    confidenceLevel: model.ConfidenceLevel,
                    confidenceLowerBoundColumn: nameof(ForecastDto.ConfidenceLowerBound),
                    confidenceUpperBoundColumn: nameof(ForecastDto.ConfidenceUpperBound)                   
                   )
                );


            ITransformer forcastTransformer = pipline.Fit(dataView);

            var forcastEngine = forcastTransformer.CreateTimeSeriesEngine<OrderSalesReportDto, ForecastDto>(mlContext);

            return forcastEngine.Predict();
        }

        public  async Task<Result<OrderSummaryReport>> GetOrderSummary(CancellationToken cancellationToken = default)
        {
            var query = _orderDbContext.Query<OrderStateEntity>()
                .AsNoTracking();


            var orderSummaryReport = new OrderSummaryReport
            {
                TotalUnPayed = await query.CountAsync(x => x.CurrentState == OrderStatusConst.Submited),
                TotalUnApproved = await query.CountAsync(x => x.CurrentState == OrderStatusConst.Accepted),
                TotalUnfullfilled = await query.CountAsync(x => x.CurrentState == OrderStatusConst.Approved),
                TotalUnShipped = await query.CountAsync(x => x.CurrentState == OrderStatusConst.Approved),
                TotalCompleted = await query.CountAsync(x => x.CurrentState == OrderStatusConst.Completed),
                TotalCancelled = await query.CountAsync(x => x.CurrentState == OrderStatusConst.Cancelled)
            };

            return orderSummaryReport;

        }
    }

 
}
