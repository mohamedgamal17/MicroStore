using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Ordering;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Reports;
using MicroStore.Client.PublicWeb.Security;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using System.Net;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Controllers
{
    [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser, Roles = ApplicationSecurityRoles.Admin)]
    public class ReportsController : AdministrationController
    {
        private readonly OrderAnalysisService _orderAnalysisService;

        private readonly CountryAnalysisService _countryAnalysisService;
        public ReportsController(OrderAnalysisService orderAnalysisService, CountryAnalysisService countryAnalysisService)
        {
            _orderAnalysisService = orderAnalysisService;
            _countryAnalysisService = countryAnalysisService;
        }


        public async Task<IActionResult> Sales()
        {
            return View(new OrderSalesReportModel());
        }
        [HttpPost]
        public async Task<IActionResult> Sales(OrderSalesReportModel model)
        {

            var response = await _orderAnalysisService.GetOrderSalesReport(new OrderSalesReportRequestOptions
            {
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Period = model.GroupBy,
                Status = OrderState.Completed.ToString(),
                Length= model.Length,
                Skip = model.Skip
            });



            var responseModel = new OrderSalesReportModel
            {
                Start = response.Skip,
                Length = response.Lenght,
                Draw = model.Draw,
                RecordsTotal = response.TotalCount,
                Data = ObjectMapper.Map<List<OrderSalesReport>, List<OrderSalesReportVM>>(response.Items)
            };

            return Json(responseModel);
        }


        public async Task<IActionResult> SalesForecasting()
        {
            try
            {
                var forcastedValues = await _orderAnalysisService.Forecast(new ForecastRequestOptions
                {
                    Horizon = 6,
                    ConfidenceLevel = 0.95f
                });

                var lastYearDate = DateTime.Now.AddMonths(-12);

                var startDate = new DateTime(lastYearDate.Year, lastYearDate.Month, 1);
                var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);

                var monthlySalesReport = await _orderAnalysisService.GetOrderSalesReport(new OrderSalesReportRequestOptions
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    Period = ReportPeriod.Monthly,
                    Status = OrderState.Completed.ToString(),
                    Length = 12
                });


                var projection = monthlySalesReport.Items.OrderBy(x => x.Date).Select(x => new OrderSalesChartDataModel
                {
                    TotalOrders = x.TotalOrders,
                    SumShippingTotalCost = x.TotalShippingPrice,
                    SumTaxTotalCost = x.TotalTaxPrice,
                    SumTotalCost = x.TotalPrice,
                    Date = x.Date.ToString("MM-dd-yyyy"),
                    
                }).ToList();

                int monthOffset = 1;

                for (int i = 0; i < forcastedValues.ForecastedValues.Length; i++)
                {

                    projection.Add(new OrderSalesChartDataModel
                    {
                        SumTotalCost = forcastedValues.ForecastedValues[i],
                        Date = DateTime.Now.AddMonths(monthOffset).ToString("MM-dd-yyyy"),
                        IsForecasted = true
                    });

                    monthOffset++;
                }

                return View(projection);
            }
            catch(MicroStoreClientException ex) when(ex.StatusCode == HttpStatusCode.BadRequest)
            {
                NotificationManager.Error(ex.Error.Title);

                return RedirectToAction(nameof(Sales));
            }
        }



        public IActionResult CountriesSales()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CountriesSales(CountrySalesSummaryModel model)
        {

            var requestOptions = new PagingReqeustOptions()
            {
                Skip = model.Skip,
                Length= model.Length
            };

            var response = await _countryAnalysisService.GetCountriesSalesSummary(requestOptions);


            var responseModel = new CountrySalesSummaryModel
            {
                Data = ObjectMapper.Map<List<CountrySalesSummary>, List<CountrySalesSummaryVM>>(response.Items),
                Start = response.Skip,
                Length = response.Lenght,
                RecordsTotal = response.TotalCount,
                Draw = model.Draw
            };

            return Ok(responseModel);
        }




    }
}
