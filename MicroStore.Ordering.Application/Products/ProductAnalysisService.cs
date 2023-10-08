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
namespace MicroStore.Ordering.Application.Products
{
    public class ProductAnalysisService : OrderApplicationService ,IProductAnalysisService
    {
        private readonly IOrderDbContext _orderDbContext;

        public ProductAnalysisService(IOrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task<Result<ForecastDto>> ForecastPrdocut(string productId,ForecastModel model ,CancellationToken cancellationToken = default)
        {

            bool canProductForecasted = await CheckIfProductCanMonthlyForecasted(productId, cancellationToken);

            if (!canProductForecasted)
            {
                return new Result<ForecastDto>(new UserFriendlyException("Current item cannot be forecasted yet.It will be forecasted after 12 months"));
            }


            var salesReport = await PrepareProductMonthlySalesReport(productId, includeCurrentMonth: false, cancellationToken);


            var forcast = PreapreForcastedModel(model,salesReport.Reports);

            return forcast;          
        }

        private ForecastDto PreapreForcastedModel(ForecastModel model, List<MonthlyReportDto> reports)
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

        private async Task<AggregatedMonthlyReportDto> PrepareProductMonthlySalesReport(string productId, bool includeCurrentMonth = true ,CancellationToken cancellationToken = default)
        {
            var query = _orderDbContext.Query<OrderStateEntity>()
                   .Where(x => x.CurrentState == OrderStatusConst.Completed)
                   .Where(x => x.OrderItems.Any(x => x.ExternalProductId == productId));

            if (!includeCurrentMonth)
            {
                query = query.Where(x => x.SubmissionDate < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
            }

            DateTime minDate = await query.MinAsync(x => x.SubmissionDate);

            DateTime maxDate = await query.MaxAsync(x => x.SubmissionDate);

            var projection = from item in query
                             group item by new
                             {
                                 item.SubmissionDate.Year,
                                 item.SubmissionDate.Month,
                                 item.OrderItems.First().ExternalProductId
                             } into grouped
                             select new MonthlyReportDto
                             {
                                 Year = grouped.Key.Year,
                                 Month = grouped.Key.Month,
                                 Min = grouped.Min(x => x.OrderItems.First().Quantity),
                                 Max = grouped.Max(x => x.OrderItems.First().Quantity),
                                 Sum = grouped.Sum(x => x.OrderItems.First().Quantity),
                                 Average = (float)grouped.Average(x => x.OrderItems.First().Quantity),
                                 Count = grouped.SelectMany(x => x.OrderItems).Count(),

                             };


            var allDates = Enumerable.Range(0, (maxDate.Year - minDate.Year) * 12 + maxDate.Month - minDate.Month + 1)
                .Select(offset => minDate.AddMonths(offset))
                .ToList();


            var salesReport = from date in allDates
                              join pr in await projection.ToListAsync()
                              on new { date.Year, date.Month } equals new { pr.Year, pr.Month } into grouped
                              from gr in grouped.DefaultIfEmpty(new MonthlyReportDto())
                              select new MonthlyReportDto
                              {
                                  Year = date.Year,
                                  Month = date.Month,
                                  Min = gr.Min,
                                  Max = gr.Max,
                                  Sum = gr.Sum,
                                  Average = gr.Average,
                                  Count = gr.Count
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


       
        private async Task<bool> CheckIfProductCanMonthlyForecasted(string productId, CancellationToken cancellationToken )
        {
            var query = _orderDbContext.Query<OrderStateEntity>()
               .Where(x => x.CurrentState == OrderStatusConst.Completed)
               .Where(x => x.OrderItems.Any(x => x.ExternalProductId == productId))
               .Where(x => x.SubmissionDate < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));

            var minDate = await query.MinAsync(x => x.SubmissionDate, cancellationToken);
            var maxDate = await query.MaxAsync(x => x.SubmissionDate, cancellationToken);

            return maxDate > minDate.AddYears(1);
               
        }

        public async Task<Result<List<ProductSummaryReport>>> GetProductSummaryReport(string productId, ProductSummaryReportModel model, CancellationToken cancellationToken = default)
        {
            var endDate = model.EndDate ?? DateTime.Now;
            var startDate = model.StartDate ?? endDate.Date.AddDays(-14);

            var query = _orderDbContext.Query<OrderStateEntity>()
               .AsNoTracking()
               .Where(x => x.CurrentState == OrderStatusConst.Completed)
               .Where(x => x.OrderItems.Any(x => x.ExternalProductId == productId))
               .Where(x => x.SubmissionDate <= endDate && x.SubmissionDate >= startDate);


            var projection = model.Period switch
            {
                ReportPeriod.Daily => query.GroupBy(x => new
                {
                    x.OrderItems.First().ExternalProductId,
                    x.SubmissionDate.Year,
                    x.SubmissionDate.Month,
                    x.SubmissionDate.Day
                }).ProjectToProductSummaryReport(),

                ReportPeriod.Monthly => query.GroupBy(x => new
                {
                    x.OrderItems.First().ExternalProductId,
                    x.SubmissionDate.Year,
                    x.SubmissionDate.Month,
                }).ProjectToProductSummaryReport(),

                _ => query.GroupBy(x => new
                {
                    x.OrderItems.First().ExternalProductId,
                    x.SubmissionDate.Year,
                }).ProjectToProductSummaryReport()
            };


           return await projection.ToListAsync();
        }
    }
}
