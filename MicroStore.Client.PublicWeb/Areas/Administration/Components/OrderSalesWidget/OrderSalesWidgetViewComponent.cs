﻿using Microsoft.AspNetCore.Mvc;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Ordering;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using System.Net;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Components.OrderSalesWidget
{
    [Widget(
        AutoInitialize = true
        )]
    public class OrderSalesWidgetViewComponent : AbpViewComponent
    {
        private readonly OrderAnalysisService _orderAnaylsisService;

        private readonly ILogger<OrderSalesWidgetViewComponent> _logger;
        public OrderSalesWidgetViewComponent(OrderAnalysisService orderAnaylsisService, ILogger<OrderSalesWidgetViewComponent> logger)
        {
            _orderAnaylsisService = orderAnaylsisService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new OrderSalesWidgetViewComponentModel
            {
                Daily = await PrepareDailySalesReport(),
                Monthly = await PrepareMonthlySalesReport(),
                Yearly = await PrepareYearlySalesReport(),
            };

            return View(model);
        }

        private async Task<List<OrderSalesChartDataModel>> PrepareDailySalesReport()
        {
            var requestOptions = new OrderSalesReportRequestOptions
            {
                Status = OrderState.Completed.ToString(),
                StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                EndDate = DateTime.Now,
                Period = ReportPeriod.Daily
            };
            var report = await _orderAnaylsisService.GetOrderSalesReport(requestOptions);

            var projection = report.Select(x => new OrderSalesChartDataModel
            {
                TotalOrders = x.TotalOrders,
                SumShippingTotalCost = x.SumShippingTotalCost,
                SumSubTotalCost = x.SumTotalCost,
                SumTaxTotalCost = x.SumTotalCost,
                SumTotalCost = x.SumTotalCost,
                Date = x.Date
            }).ToList();

            return projection;
        }


        private async Task<List<OrderSalesChartDataModel>> PrepareMonthlySalesReport()
        {
            var requestOptions = new OrderSalesReportRequestOptions
            {
                Status = OrderState.Completed.ToString(),
                StartDate = new DateTime(DateTime.Now.Year, 1, 1),
                EndDate = DateTime.Now,
                Period = ReportPeriod.Monthly
            };
            var report = await _orderAnaylsisService.GetOrderSalesReport(requestOptions);

            var projection = report.Select(x => new OrderSalesChartDataModel
            {
                TotalOrders = x.TotalOrders,
                SumShippingTotalCost = x.SumShippingTotalCost,
                SumSubTotalCost = x.SumTotalCost,
                SumTaxTotalCost = x.SumTotalCost,
                SumTotalCost = x.SumTotalCost,
                Date = x.Date
            }).ToList();

            try
            {
                var forecastRequestOptions = new ForecastRequestOptions
                {
                    ConfidenceLevel = 0.95f,
                    Horizon = 4
                };

                var forecastedReponse = await _orderAnaylsisService.Forecast(forecastRequestOptions);

                int monthOffset = 1;

                for (int i = 0; i < forecastedReponse.ForecastedValues.Length; i++)
                {

                    projection.Add(new OrderSalesChartDataModel
                    {
                        SumTotalCost = forecastedReponse.ForecastedValues[i],
                        Date = DateTime.Now.AddMonths(monthOffset).ToString("MM-dd-yyyy"),
                        IsForecasted = true
                    });

                    monthOffset++;
                }

            }
            catch (MicroStoreClientException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                _logger.LogDebug("Unable to get order forecasted value yet");
            }

            return projection;
        }

        private async Task<List<OrderSalesChartDataModel>> PrepareYearlySalesReport()
        {
            var requestOptions = new OrderSalesReportRequestOptions
            {
                Status = OrderState.Completed.ToString(),
                StartDate = new DateTime(DateTime.Now.AddYears(-10).Year,10 , 1),
                EndDate = DateTime.Now,
                Period = ReportPeriod.Yearly
            };

            var report = await _orderAnaylsisService.GetOrderSalesReport(requestOptions);


            var projection = report.Select(x => new OrderSalesChartDataModel
            {
                TotalOrders = x.TotalOrders,
                SumShippingTotalCost = x.SumShippingTotalCost,
                SumSubTotalCost = x.SumTotalCost,
                SumTaxTotalCost = x.SumTotalCost,
                SumTotalCost = x.SumTotalCost,
                Date = x.Date
            }).ToList();


            return projection;
        }
    }

    public class OrderSalesWidgetViewComponentModel
    {
        public List<OrderSalesChartDataModel> Daily { get; set; }
        public List<OrderSalesChartDataModel> Monthly { get; set; }
        public List<OrderSalesChartDataModel> Yearly { get; set; }
    }
}
