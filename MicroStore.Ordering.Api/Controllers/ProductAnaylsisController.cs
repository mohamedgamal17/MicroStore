using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Ordering.Application.Models;
using MicroStore.Ordering.Application.Products;
namespace MicroStore.Ordering.Api.Controllers
{
    [Route("api/anaylsis/products")]
    [ApiController]
   // [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser)]
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



        [HttpGet]
        [Route("{productId}/units-summary")]
        public async Task<IActionResult> GetProductSummaryReport(string productId , ProductSummaryReportModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return InvalideModelState();
            }

            var result = await _productAnalysisService.GetProductSummaryReport(productId, model);

            return result.ToOk();
        }


    }
}
