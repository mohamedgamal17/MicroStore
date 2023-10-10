using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Ordering.Application.Common;
using MicroStore.Ordering.Application.Domain;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Extensions;
using MicroStore.Ordering.Application.Models;
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
                return new Result<ForecastDto>(new UserFriendlyException("Current item cannot be forecasted yet.It must has at least 12 months history in series"));
            }


            var salesReport = await PrepareProductMonthlySalesReport(productId, includeCurrentMonth: false, cancellationToken);


            var forcast = PreapreForcastedModel(model,salesReport);

            return forcast;          
        }

        private ForecastDto PreapreForcastedModel(ForecastModel model, List<ProductSalesReportDto> reports)
        {
            var mlContext = new MLContext();

            var dataView = mlContext.Data.LoadFromEnumerable(reports);

            var pipline = mlContext.Transforms.Conversion.ConvertType(inputColumnName: nameof(ProductSalesReportDto.Quantity), outputColumnName: nameof(ProductSalesReportDto.Quantity), outputKind: Microsoft.ML.Data.DataKind.Single)
               .Append(
               mlContext.Forecasting.ForecastBySsa(
                   inputColumnName: nameof(ProductSalesReportDto.Quantity),
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

        private async Task<List<ProductSalesReportDto>> PrepareProductMonthlySalesReport(string productId, bool includeCurrentMonth = true ,CancellationToken cancellationToken = default)
        {
            var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var startDate = endDate.AddYears(-1);

            var query = _orderDbContext.Query<ProductSalesReport>()
                   .AsNoTracking()
                   .Where(x => x.ProductId == productId)
                   .Where(x=> x.Date >= startDate && x.Date <= endDate);


            var projection = from item in query
                             group item by new
                             {
                                 item.Date.Year,
                                 item.Date.Month,
                                 item.ProductId
                             } into grouped
                             select new ProductSalesReportDto
                             {
                                 Quantity = grouped.Sum(x => x.Quantity),
                                 TotalPrice = grouped.Sum(x => x.TotalPrice),
                                 Date = grouped.Max(x => x.Date),
                             };


            return await projection.OrderBy(x=> x.Date).ToListAsync(cancellationToken);                            
        }


       
        private async Task<bool> CheckIfProductCanMonthlyForecasted(string productId, CancellationToken cancellationToken )
        {
            var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var startDate = endDate.AddYears(-1);

            var count = await _orderDbContext.Query<ProductSalesReport>()
                   .AsNoTracking()
                   .Where(x => x.ProductId == productId)
                   .Where(x => x.Date >= startDate && x.Date <= endDate)
                   .GroupBy(x => new { x.Date.Year, x.Date.Month })
                   .CountAsync();
                   

            return count >= 12;
               
        }

        public async Task<Result<List<ProductSalesReportDto>>> GetProductSalesReport(string productId, ProductSummaryReportModel model, CancellationToken cancellationToken = default)
        {
            var endDate = model.EndDate ?? DateTime.Now;
            var startDate = model.StartDate ?? endDate.Date.AddDays(-14);

            var query = _orderDbContext.Query<ProductSalesReport>()
               .AsNoTracking()
               .Where(x => x.ProductId == productId)
               .Where(x => x.Date <= endDate && x.Date >= startDate);


            var projection = model.Period switch
            {
                ReportPeriod.Daily => query.Select(x=> new ProductSalesReportDto
                {
                    Quantity = x.Quantity,
                    TotalPrice =x.TotalPrice,
                    Date = x.Date
                }),

                ReportPeriod.Monthly => query.GroupBy(x => new
                {
                    x.Date.Year,
                    x.Date.Month,
                }).Select(x=> new ProductSalesReportDto
                {
                    Quantity = x.Sum(x=> x.Quantity),
                    TotalPrice = x.Sum(x => x.TotalPrice),
                    Date = x.Max(x=> x.Date)
                }),
                _ => query.GroupBy(x => new 
                {
                    x.Date.Year,
                }).Select(x => new ProductSalesReportDto
                {
                    Quantity = x.Sum(x => x.Quantity),
                    TotalPrice = x.Sum(x => x.TotalPrice),
                    Date = x.Max(x => x.Date)
                })
            };


           return await projection.OrderBy(x=> x.Date).ToListAsync(cancellationToken);
        }

        public async Task<Result<PagedResult<BestSellerReport>>> GetBestSellersReport(PagingAndSortingQueryParams queryParams, CancellationToken cancellationToken = default)
        {

            var query = _orderDbContext.Query<BestSellerReport>()
               .AsNoTracking();

            query = queryParams.SortBy?.ToLower() switch
            {
                "amount" => queryParams.Desc ? query.OrderByDescending(x => x.Amount) : query.OrderBy(x => x.Amount),
                "quantity" => queryParams.Desc ? query.OrderByDescending(x => x.Quantity) : query.OrderBy(x => x.Quantity),
                _ => query.OrderByDescending(x => x.Quantity)
            };

            return await query.PageResult(queryParams.Skip, queryParams.Length, cancellationToken);
        }
    }
}
