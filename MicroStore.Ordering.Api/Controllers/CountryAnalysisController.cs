using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Ordering.Application.Countries;
using MicroStore.Ordering.Application.Models;
using MicroStore.Ordering.Application.Security;

namespace MicroStore.Ordering.Api.Controllers
{
    [Route("api/anaylsis/countries")]
    [ApiController]
    [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser)]
    public class CountryAnalysisController : MicroStoreApiController
    {
        private readonly ICountryAnalysisService _countryAnalysisService;

        public CountryAnalysisController(ICountryAnalysisService countryAnalysisService)
        {
            _countryAnalysisService = countryAnalysisService;
        }

        [HttpPost]
        [Route("{countryCode}/forecast")]
        public async Task<IActionResult> Forecast(string countryCode, [FromBody]  ForecastModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);

                return InvalideModelState();
            }

            var result = await _countryAnalysisService.ForecastSales(countryCode, model);

            return result.ToOk();
        }

        [HttpGet]
        [Route("{countryCode}/sales-report")]
        public async Task<IActionResult> GetCountrySalesReport(string countryCode, [FromQuery] CountrySalesReportModel model)
        {
            var result = await _countryAnalysisService.GetCountrySalesReport(countryCode,model);

            return result.ToOk();
        }

        [HttpGet]
        [Route("sales-summary-report")]
        public async Task<IActionResult> GetCountriesSalesSummary([FromQuery]PagingQueryParams model)
        {
            var result = await _countryAnalysisService.GetCountriesSalesSummaryReport(model);
            
            return result.ToOk();
        }
    }
}
