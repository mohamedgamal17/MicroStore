using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Common;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Extensions;
using MicroStore.Ordering.Application.Models;
using MicroStore.Ordering.Application.StateMachines;
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
                return new Result<ForecastDto>(new UserFriendlyException("Sales cannot be forecasted yet.It will be forecasted after 12 months"));
            }

            var salesReport = await PrepareSalesMonthlyReport(includeCurrentMonth: false, cancellationToken);


            var forcast = PrepareForcastedModel(model, salesReport.Reports);

            return forcast;
        }


        public async Task<Result<List<OrderSummaryReport>>> GetOrdersSummaryReport(OrderSummaryReportModel model, CancellationToken cancellationToken = default)
        {
            var query = _orderDbContext.Query<OrderStateEntity>()
                .OrderBy(x => x.SubmissionDate)
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


            query = query.Where(x => x.SubmissionDate >= startDate && x.SubmissionDate <= endDate);

            var projection = model.Period switch
            {
                ReportPeriod.Daily => query.GroupBy(x => x.SubmissionDate).ProjectToOrderSummaryReport(),

                ReportPeriod.Monthly => query.GroupBy(x => new
                {
                    x.SubmissionDate.Month,
                    x.SubmissionDate.Year
                })
                .ProjectToOrderSummaryReport(),

                _ => query.GroupBy(x => new { x.SubmissionDate.Year }).OrderBy(x => x.Key.Year).ProjectToOrderSummaryReport()
            };

            return await projection.ToListAsync(cancellationToken);

        }

        private async Task<AggregatedMonthlyReportDto> PrepareSalesMonthlyReport(bool includeCurrentMonth = true ,CancellationToken cancellationToken = default)
        {

            var query = _orderDbContext.Query<OrderStateEntity>()
                .AsNoTracking()
                .Where(x => x.CurrentState == OrderStatusConst.Completed);


            if (!includeCurrentMonth)
            {
                var currentMonthDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                query = query.Where(x => x.SubmissionDate < currentMonthDate);
            }

            var minDate = await query.MinAsync(x => x.SubmissionDate, cancellationToken);
            var maxDate = await  query.MaxAsync(x => x.SubmissionDate, cancellationToken);

            var allDates = Enumerable.Range(0, (maxDate.Year - minDate.Year) * 12 + maxDate.Month - minDate.Month + 1)
                .Select(offset => minDate.AddMonths(offset))
                .ToList();


            var projection = from order in query
                             group order by new
                             {
                                 order.SubmissionDate.Year,
                                 order.SubmissionDate.Month,
                             } into grouped
                             select new MonthlyReportDto
                             {
                                 Year = grouped.Key.Year,
                                 Month = grouped.Key.Month,
                                 Min = (float)grouped.Min(x=> x.TotalPrice),
                                 Max = (float)grouped.Max(x=> x.TotalPrice),
                                 Average = (float)grouped.Average(x=> x.TotalPrice),
                                 Sum = (float)grouped.Sum(x=> x.TotalPrice),
                                 Count =grouped.Count()
                             };



            var salesReport = from date in allDates
                              join pr in await projection.ToListAsync()
                              on new { date.Year, date.Month } equals new { pr.Year, pr.Month }
                              select new MonthlyReportDto
                              {
                                  Year = date.Year,
                                  Month = date.Month,
                                  Min = pr.Min,
                                  Max = pr.Max,
                                  Average = pr.Average,
                                  Sum = pr.Sum,
                                  Count = pr.Count
                              };


            return new AggregatedMonthlyReportDto
            {

                Min = salesReport.Min(x => x.Sum),
                Max = salesReport.Max(x => x.Sum),
                Average = salesReport.Max(x => x.Average),
                Sum = salesReport.Sum(x => x.Sum),
                Reports = salesReport.ToList()
            };
        }


        private async Task<bool> CheckIfSalesCanMonthlyForecasted(CancellationToken cancellationToken = default)
        {

            var currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var query = _orderDbContext.Query<OrderStateEntity>()
               .Where(x => x.CurrentState == OrderStatusConst.Completed)
               .Where(x => x.SubmissionDate < currentMonth);

            var minDate = await query.MinAsync(x => x.SubmissionDate, cancellationToken);

            var maxDate = await query.MaxAsync(x => x.SubmissionDate, cancellationToken);

            return maxDate > minDate.AddYears(1);

        }

        private ForecastDto PrepareForcastedModel(ForecastModel model, List<MonthlyReportDto> reports)
        {
            var mlContext = new MLContext();

            var dataView = mlContext.Data.LoadFromEnumerable(reports);


            var pipline = mlContext.Transforms.Conversion.ConvertType(inputColumnName: nameof(MonthlyReportDto.Sum), outputColumnName: nameof(MonthlyReportDto.Sum), outputKind: Microsoft.ML.Data.DataKind.Single)
                .Append(
                mlContext.Forecasting.ForecastBySsa(                    
                    inputColumnName: nameof(MonthlyReportDto.Sum),
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

            var forcastEngine = forcastTransformer.CreateTimeSeriesEngine<MonthlyReportDto, ForecastDto>(mlContext);

            return forcastEngine.Predict();
        }

              
    }

 
}
