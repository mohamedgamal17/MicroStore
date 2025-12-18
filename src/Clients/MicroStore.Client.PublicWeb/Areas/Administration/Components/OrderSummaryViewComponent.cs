using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Ordering;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using System.Net;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.ObjectMapping;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Components
{
    public class OrderSummaryViewComponent : AbpViewComponent
    {
        private readonly OrderService _orderService;

        private readonly IObjectMapper _objectMapper;

        public OrderSummaryViewComponent(OrderService orderService, IObjectMapper objectMapper)
        {
            _orderService = orderService;
            _objectMapper = objectMapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid orderId)
        {
            try
            {
                var order = await _orderService.GetAsync(orderId);

                var viewModel = new OrderSummaryComponentViewModel
                {
                    Order = _objectMapper.Map<Order, OrderVM>(order)
                };

                return View(viewModel);
            }
            catch (MicroStoreClientException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return View(new OrderSummaryComponentViewModel { HasError = true });
            }


        }
        }


        public class OrderSummaryComponentViewModel
        {
            public bool HasError { get; set; }

            public OrderVM Order { get; set; }

        }
    }
