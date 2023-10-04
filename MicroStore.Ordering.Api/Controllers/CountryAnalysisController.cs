using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Ordering.Application.Countries;
using MicroStore.Ordering.Application.Models;
using MicroStore.Ordering.Application.Security;

namespace MicroStore.Ordering.Api.Controllers
{
    [Route("api/anaylsis/countries")]
    [ApiController]
   // [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser)]
    public class CountryAnalysisController : MicroStoreApiController
    {
        private readonly ICountryAnalysisService _countryAnalysisService;

        public CountryAnalysisController(ICountryAnalysisService countryAnalysisService)
        {
            _countryAnalysisService = countryAnalysisService;
        }

        [HttpPost]
        [Route("{countryCode}/forecast")]
        public async Task<IActionResult> Forecast(string countryCode,ForecastModel model)
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

        [HttpPost]
        [Route("{countryCode}/sales-monthly-report")]
        public async Task<IActionResult> GetMonthlyReport(string countryCode)
        {
            var result = await _countryAnalysisService.GetSalesMonthlyReport(countryCode);

            return result.ToOk();
        }

        [HttpPost]
        [Route("{countryCode}/sales-daily-report")]
        public async Task<IActionResult> GetDailyReport(string countryCode, DailyReportModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return InvalideModelState();
            }

            var result = await _countryAnalysisService.GetSalesDailyReport(countryCode,model);

            return result.ToOk();
        }
    }
}
