using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.BuildingBlocks.Utils.Paging.Params;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using Volo.Abp.Application.Services;

namespace MicroStore.Ordering.Application.Countries
{
    public interface ICountryAnalysisService : IApplicationService
    {
        Task<Result<ForecastDto>> ForecastSales(string countryCode,ForecastModel model ,CancellationToken cancellationToken = default);

        Task<Result<PagedResult<CountrySalesSummaryReportDto>>> GetCountriesSalesSummaryReport(PagingQueryParams queryParams, CancellationToken cancellationToken = default);

        Task<Result<PagedResult<CountrySalesReportDto>>> GetCountrySalesReport(string countryCode, CountrySalesReportModel model, CancellationToken cancellationToken = default);
    }
}
