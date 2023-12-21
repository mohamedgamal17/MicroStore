using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Ordering.Application.Domain;
using MicroStore.Ordering.Application.Dtos;
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


        [HttpGet]
        [Route("bestsellers")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(BestSellerReport))]
        public async Task<IActionResult> BestSellers([FromQuery]PagingAndSortingQueryParams queryParams)
        {
            var result = await _productAnalysisService.GetBestSellersReport(queryParams);

            return result.ToOk();
        }


        [HttpPost]
        [Route("{productId}/forecast")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ForecastDto))]

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
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(PagedResult<ProductSalesReportDto>))]
        public async Task<IActionResult> GetProductSummaryReport(string productId ,[FromQuery] ProductSummaryReportModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return InvalideModelState();
            }

            var result = await _productAnalysisService.GetProductSalesReport(productId, model);

            return result.ToOk();
        }


    }
}
