using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.Ordering.Application.Domain;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using MicroStore.Ordering.Application.Orders;
using MicroStore.Ordering.Application.Security;

namespace MicroStore.Ordering.Api.Controllers
{
    [Route("api/anaylsis/orders")]
    [ApiController]
    [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser)]
    public class OrderAnalysisController : MicroStoreApiController 
    {
        private readonly IOrderAnalysisService _orderAnalysisService;

        public OrderAnalysisController(IOrderAnalysisService orderAnalysisService)
        {
            _orderAnalysisService = orderAnalysisService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ForecastDto>))]
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<OrderSalesReport>))]
        [Route("sales-report")]
        public async Task<IActionResult> GetOrderSalesReport([FromQuery]OrderSalesReportModel model)
        {
  
            var result = await _orderAnalysisService.GetOrdersSalesReport(model);

            return result.ToOk();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderSummaryReport))]
        [Route("summary-report")]

        public async Task<IActionResult> GetOrderSummaryReport()
        {
            var result = await _orderAnalysisService.GetOrderSummary();

            return result.ToOk();
        }
    }
}
