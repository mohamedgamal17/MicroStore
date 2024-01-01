using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.BuildingBlocks.Utils.Paging.Params;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Ordering.Application.Domain;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.Ordering.Application.Products
{
    public interface IProductAnalysisService : IApplicationService
    {
        Task<Result<ForecastDto>> ForecastPrdocut(string productId,ForecastModel model,CancellationToken cancellationToken = default);

        Task<Result<PagedResult<ProductSalesReportDto>>> GetProductSalesReport(string productId, ProductSummaryReportModel model, CancellationToken cancellationToken = default);


        Task<Result<PagedResult<BestSellerReport>>> GetBestSellersReport(PagingAndSortingQueryParams queryParams,CancellationToken cancellationToken = default);

    }
}
