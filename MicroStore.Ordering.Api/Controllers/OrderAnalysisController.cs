using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Ordering.Application.Models;
using MicroStore.Ordering.Application.Orders;
using MicroStore.Ordering.Application.Security;

namespace MicroStore.Ordering.Api.Controllers
{
    [Route("api/anaylsis/orders")]
    [ApiController]
 //   [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser)]
    public class OrderAnalysisController : MicroStoreApiController 
    {
        private readonly IOrderAnalysisService _orderAnalysisService;

        public OrderAnalysisController(IOrderAnalysisService orderAnalysisService)
        {
            _orderAnalysisService = orderAnalysisService;
        }

        [HttpPost]
        [Route("forecast")]
        public async Task<IActionResult> Forecast(ForecastModel model)
        {
            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);

                return InvalideModelState();
            }

            var result = await _orderAnalysisService.ForecastSales(model);

            return result.ToOk();
        }


        [HttpGet]
        [Route("sales-summary")]
        public async Task<IActionResult> GetOrderSummary([FromQuery]OrderSummaryReportModel model)
        {
  
            var result = await _orderAnalysisService.GetOrdersSummaryReport(model);

            return result.ToOk();
        }
    }
}
