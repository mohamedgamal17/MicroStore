using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Ordering.Application.Models;
using MicroStore.Ordering.Application.Products;
using MicroStore.Ordering.Application.Security;
namespace MicroStore.Ordering.Api.Controllers
{
    [Route("api/anaylsis/products")]
    [ApiController]
    [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser)]
    public class ProductAnaylsisController : MicroStoreApiController
    {
        private readonly IProductAnalysisService _productAnalysisService;

        public ProductAnaylsisController(IProductAnalysisService productAnalysisService)
        {
            _productAnalysisService = productAnalysisService;
        }

        [HttpPost]
        [Route("{productId}/forecast")]
        public async Task<IActionResult> Forecast(string productId ,ForecastModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return InvalideModelState();
            }

            var result = await _productAnalysisService.ForecastPrdocut(productId, model);

            return result.ToOk();
        }

        [HttpPost]
        [Route("{productId}/monthly-report")]
        public async Task<IActionResult> GetMonthlyReport(string productId) 
        {
            var result = await _productAnalysisService.GetProductMonthlyReport(productId);

            return result.ToOk();
        }

        [HttpPost]
        [Route("{productId}/daily-report")]
        public async Task<IActionResult> GetDailyReport(string productId , DailySalesReportModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return InvalideModelState();
            }

            var result = await _productAnalysisService.GetProductDailySalesReport(productId, model);

            return result.ToOk();
        }


    }
}
