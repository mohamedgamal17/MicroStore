using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Common;
using MicroStore.Ordering.Application.Domain;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using Volo.Abp;
namespace MicroStore.Ordering.Application.Countries
{
    public class CountryAnalysisService : OrderApplicationService ,ICountryAnalysisService
    {
        private readonly IOrderDbContext _orderDbContext;

        public CountryAnalysisService(IOrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task<Result<ForecastDto>> ForecastSales(string countryCode,ForecastModel model ,CancellationToken cancellationToken = default)
        {
            bool canForecastSales = await CheckIfSalesCanMonthlyForecasted(countryCode,cancellationToken);

            if (!canForecastSales)
            {
                return new Result<ForecastDto>(new UserFriendlyException("Sales cannot be forecasted yet.It will be forecasted after 12 months"));
            }

            var salesReport = await PrepareSalesMonthlyReport(countryCode,includeCurrentMonth: false, cancellationToken);


            var forcast = PrepareForcastedModel(model, salesReport.Reports);

            return forcast;
        }

        public async Task<Result<AggregateDailyReportDto>> GetSalesDailyReport(string countryCode, DailyReportModel model, CancellationToken cancellationToken = default)
        {
            var report = await PrepreSalesDailyReport(countryCode,model.Year, model.Month, cancellationToken: cancellationToken);

            return report;
        }

        public async Task<Result<AggregatedMonthlyReportDto>> GetSalesMonthlyReport(string countryCode, CancellationToken cancellationToken = default)
        {
            var report = await PrepareSalesMonthlyReport(countryCode, cancellationToken: cancellationToken);

            return report;
        }


        private async Task<AggregatedMonthlyReportDto> PrepareSalesMonthlyReport(string countryCode,bool includeCurrentMonth = true, CancellationToken cancellationToken = default)
        {

            var query = _orderDbContext.Query<OrderStateEntity>()
                .AsNoTracking()
                .Where(x => x.CurrentState == OrderStatusConst.Completed)
                .Where(x => x.ShippingAddress.CountryCode == countryCode);


            if (!includeCurrentMonth)
            {
                query = query.Where(x => x.SubmissionDate < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
            }

            var minDate = await query.MinAsync(x => x.SubmissionDate, cancellationToken);
            var maxDate = await query.MaxAsync(x => x.SubmissionDate, cancellationToken);

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
                                 Min = (float)grouped.Min(x => x.TotalPrice),
                                 Max = (float)grouped.Max(x => x.TotalPrice),
                                 Average = (float)grouped.Average(x => x.TotalPrice),
                                 Sum = (float)grouped.Sum(x => x.TotalPrice),
                                 Count = grouped.Count()
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

        private async Task<AggregateDailyReportDto> PrepreSalesDailyReport(string countryCode,int year, int month, bool includeCurrentDay = false, CancellationToken cancellationToken = default)
        {
            var query = _orderDbContext.Query<OrderStateEntity>()
                .AsNoTracking()
                .Where(x => x.CurrentState == OrderStatusConst.Completed)
                .Where(x => x.SubmissionDate.Year == year && x.SubmissionDate.Month == month)
                .Where(x => x.ShippingAddress.CountryCode == countryCode);


            if (!includeCurrentDay)
            {
                query = query.Where(x => x.SubmissionDate < DateTime.Now);
            }

            DateTime minDate = await query.MinAsync(sr => sr.SubmissionDate);

            DateTime maxDate = await query.MaxAsync(sr => sr.SubmissionDate);

            List<DateTime> allDates = Enumerable.Range(0, (maxDate - minDate).Days + 1)
                .Select(offset => minDate.AddDays(offset))
                .ToList();

            var projection = from order in query
                             group order by new
                             {
                                 order.SubmissionDate.Year,
                                 order.SubmissionDate.Month,
                                 order.SubmissionDate.Day
                             } into grouped
                             select new DailyReportDto
                             {
                                 Year = grouped.Key.Year,
                                 Month = grouped.Key.Month,
                                 Day = grouped.Key.Day,
                                 Min = grouped.Min(x => x.TotalPrice),
                                 Max = grouped.Max(x => x.TotalPrice),
                                 Average = grouped.Average(x => x.TotalPrice),
                                 Sum = grouped.Sum(x => x.TotalPrice),
                                 Count = grouped.Count()
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


        private async Task<bool> CheckIfSalesCanMonthlyForecasted(string countryCode , CancellationToken cancellationToken = default)
        {
            var query = _orderDbContext.Query<OrderStateEntity>()
               .Where(x => x.CurrentState == OrderStatusConst.Completed)
               .Where(x => x.ShippingAddress.CountryCode == countryCode)
               .Where(x => x.SubmissionDate < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));

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
