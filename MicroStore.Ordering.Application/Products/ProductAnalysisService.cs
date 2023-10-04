using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Common;
using MicroStore.Ordering.Application.Dtos;
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

        public async Task<Result<AggregatedMonthlyReportDto>> GetProductMonthlyReport(string productId, CancellationToken cancellationToken = default)
        {
            var salesReport = await PrepareProductMonthlySalesReport(productId, cancellationToken : cancellationToken);

            return salesReport;
        }


        public async Task<Result<AggregateDailyReportDto>> GetProductDailySalesReport(string productId, DailyReportModel model, CancellationToken cancellationToken = default)
        {
            var salesReport = await PreapreProductDailySalesReport(productId, model.Year, model.Month, cancellationToken: cancellationToken);

            return salesReport;
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


        private async Task<AggregateDailyReportDto> PreapreProductDailySalesReport(string productId, int year , int month,bool includeCurrentDay = true , CancellationToken cancellationToken = default)
        {
            var query = _orderDbContext.Query<OrderStateEntity>()
               .Where(x => x.CurrentState == OrderStatusConst.Completed)
               .Where(x => x.OrderItems.Any(x => x.ExternalProductId == productId))
               .Where(x => x.SubmissionDate.Year == year && x.SubmissionDate.Month == month);

            if (!includeCurrentDay)
            {
                query = query.Where(x => x.SubmissionDate < DateTime.Now);
            }

            DateTime minDate = await query.MinAsync(sr => sr.SubmissionDate);
            DateTime maxDate = await query.MaxAsync(sr => sr.SubmissionDate);

            List<DateTime> allDates = Enumerable.Range(0, (maxDate - minDate).Days + 1)
                .Select(offset => minDate.AddDays(offset))
                .ToList();

            var projection = from item in query
                             group item by new
                             {
                                 item.SubmissionDate.Year,
                                 item.SubmissionDate.Month,
                                 item.SubmissionDate.Day,
                                 item.OrderItems.First().ExternalProductId
                             } into grouped
                             select new DailyReportDto
                             {
                                 Year = grouped.Key.Year,
                                 Month = grouped.Key.Month,
                                 Day = grouped.Key.Day,
                                 Min = grouped.Min(x => x.OrderItems.First().Quantity),
                                 Max = grouped.Max(x => x.OrderItems.First().Quantity),
                                 Sum = grouped.Sum(x => x.OrderItems.First().Quantity),
                                 Average = grouped.Average(x => x.OrderItems.First().Quantity),
                                 Count = grouped.SelectMany(x => x.OrderItems).Count(),
                             };

            var salesReport = from date in allDates
                              join pr in await projection.ToListAsync()
                              on new { date.Year, date.Month, date.Day } equals new { pr.Year, pr.Month, pr.Day } into grouped
                              from grp in grouped.DefaultIfEmpty(new DailyReportDto())
                              select new DailyReportDto
                              {
                                  Year = date.Year,
                                  Month = date.Month,
                                  Day = date.Day,
                                  Min = grp.Min,
                                  Max = grp.Max,
                                  Sum = grp.Sum,
                                  Average = grp.Average,
                                  Count = grp.Count
                              };


            return new AggregateDailyReportDto
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
    }
}
