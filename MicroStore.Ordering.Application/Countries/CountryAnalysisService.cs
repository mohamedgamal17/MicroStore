using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.BuildingBlocks.Utils.Paging.Extensions;
using MicroStore.BuildingBlocks.Utils.Paging.Params;
using MicroStore.BuildingBlocks.Utils.Results;
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
                return new Result<ForecastDto>(new UserFriendlyException("Sales cannot be forecasted yet.It will be forecasted after 36 months"));
            }

            var salesReport = await PrepareSalesMonthlyReport(countryCode, cancellationToken);


            var forcast = PrepareForcastedModel(model, salesReport);

            return forcast;
        }

        public async Task<Result<PagedResult<CountrySalesSummaryReportDto>>> GetCountriesSalesSummaryReport(PagingQueryParams queryParams, CancellationToken cancellationToken = default)
        {
            var query = _orderDbContext.Query<CountrySalesSummaryReport>()
                .AsNoTracking()
                .ProjectTo<CountrySalesSummaryReportDto>(MapperAccessor.Mapper.ConfigurationProvider);

            return await query.PageResult(queryParams.Skip, queryParams.Length, cancellationToken);

        }

        public async Task<Result<PagedResult<CountrySalesReportDto>>> GetCountrySalesReport(string countryCode, CountrySalesReportModel model ,CancellationToken cancellationToken = default)
        {

            var query = _orderDbContext.Query<CountrySalesReport>()
                .AsNoTracking();

            if(model.StartDate != null)
            {
                query = query.Where(x => x.Date >= model.StartDate);
            }

            if(model.EndDate != null)
            {
                query = query.Where(x => x.Date <= model.EndDate);
            }

            query = query.Where(x => x.CountryCode == countryCode);

            var projection = model.Period switch
            {
                ReportPeriod.Daily => query.Select(x => new CountrySalesReportDto
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
                }).Select(x => new CountrySalesReportDto
                {
                    Date = x.Max(x => x.Date),
                    TotalOrders = x.Sum(x => x.TotalOrders),
                    TotalShippingPrice = x.Sum(x => x.TotalShippingPrice),
                    TotalTaxPrice = x.Sum(x => x.TotalTaxPrice),
                    TotalPrice = x.Sum(x => x.TotalPrice),
                }),
                _ => query.GroupBy(x => new { x.Date.Year })
                .Select(x => new CountrySalesReportDto
                {
                    Date = x.Max(x => x.Date),
                    TotalOrders = x.Sum(x => x.TotalOrders),
                    TotalShippingPrice = x.Sum(x => x.TotalShippingPrice),
                    TotalTaxPrice = x.Sum(x => x.TotalTaxPrice),
                    TotalPrice = x.Sum(x => x.TotalPrice),
                })
            };

            return await projection.OrderByDescending(x=> x.Date).PageResult(model.Skip, model.Length, cancellationToken);
        }


        private async Task<List<CountrySalesReportDto>> PrepareSalesMonthlyReport(string countryCode ,  CancellationToken cancellationToken = default)
        {
            var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var startDate = endDate.AddYears(-3);

            var query = _orderDbContext.Query<CountrySalesReport>()
                .AsNoTracking()
                .Where(x=> x.CountryCode == countryCode)
                .Where(x => x.Date >= startDate && x.Date < endDate);

            var projection = from rp in query
                             group rp by new
                             {
                                 rp.Date.Year,
                                 rp.Date.Month,
                             } into grouped
                             select new CountrySalesReportDto
                             {

                                 TotalOrders = grouped.Sum(x => x.TotalOrders),
                                 TotalShippingPrice = grouped.Sum(x => x.TotalShippingPrice),
                                 TotalTaxPrice = grouped.Sum(x => x.TotalTaxPrice),
                                 TotalPrice = grouped.Sum(x => x.TotalPrice),
                                 Date = grouped.Max(x => x.Date),
                             };

            return await projection.ToListAsync(cancellationToken);
        }

   

        private async Task<bool> CheckIfSalesCanMonthlyForecasted(string countryCode , CancellationToken cancellationToken = default)
        {
            var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var startDate = endDate.AddYears(-3);

            var count = await _orderDbContext.Query<CountrySalesReport>()
               .Where(x=> x.CountryCode == countryCode)
               .Where(x => x.Date < endDate && x.Date >= startDate)
               .GroupBy(x => new { x.Date.Year, x.Date.Month })
               .CountAsync();

            return count >= 36;

        }

        private ForecastDto PrepareForcastedModel(ForecastModel model, List<CountrySalesReportDto> reports)
        {
            var mlContext = new MLContext();

            var dataView = mlContext.Data.LoadFromEnumerable(reports);

            var pipline = mlContext.Transforms.Conversion.ConvertType(inputColumnName: nameof(CountrySalesReportDto.TotalPrice), outputColumnName: nameof(CountrySalesReportDto.TotalPrice), outputKind: Microsoft.ML.Data.DataKind.Single)
                   .Append(
                   mlContext.Forecasting.ForecastBySsa(
                       inputColumnName: nameof(CountrySalesReportDto.TotalPrice),
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

            var forcastEngine = forcastTransformer.CreateTimeSeriesEngine<CountrySalesReportDto, ForecastDto>(mlContext);

            return forcastEngine.Predict();
        }

    }
}
