using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Products;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Ordering;
using MicroStore.Client.PublicWeb.Extensions;
using MicroStore.Client.PublicWeb.Security;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping;
using System.Net;
using static IdentityModel.OidcConstants;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Controllers
{
    [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser, Roles = ApplicationSecurityRoles.Admin)]
    public class OrderController : AdministrationController
    {

        private readonly OrderService _orderService;

        private readonly OrderAggregateService _orderAggregateService;

        private readonly ProductService _productService;

        private readonly ShipmentService _shipmentService;
        public OrderController(OrderService orderService, OrderAggregateService orderAggregateService, ProductService productService, ShipmentService shipmentService)
        {
            _orderService = orderService;
            _orderAggregateService = orderAggregateService;
            _productService = productService;
            _shipmentService = shipmentService;
        }

        public async Task<IActionResult> Index()
        {
            return View(new OrderSearchModel());
        }


        [HttpPost]
        public async Task<IActionResult> Index(OrderSearchModel model)
        {
            var requestOptions = new OrderListRequestOptions
            {
                Length = model.PageSize,
                Skip = model.Skip,
                StartSubmissionDate = model.StartSubmissionDate,
                EndSubmissionDate = model.EndSubmissionDate,
                OrderNumber  = model.OrderNumber,
                States = model.States != null  ? model.States.Select(x=>x.ToString()).JoinAsString(",") : null,
                SortBy = "submission_date",
                Desc = true
            };

            var response = await _orderAggregateService.ListAsync(requestOptions);

            var responseModel = new OrderListModel
            {
                Draw = model.Draw,
                Length = response.Lenght,
                Start = response.Skip,
                RecordsTotal = response.TotalCount,
                Data = ObjectMapper.Map<List<OrderAggregate>, List<OrderAggregateVM>>(response.Items)
            };

            return Json(responseModel);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var orderAggregate = await _orderAggregateService.GetAsync(id);

            return View(ObjectMapper.Map<OrderAggregate, OrderAggregateVM>(orderAggregate));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderShipment(CreateOrderShipmmentModel model)
        {

            try
            {

                var requestOptions = await PrepareShipmentCreateRequestOptions(model.OrderId);

                var shipment = await _shipmentService.CreateAsync(requestOptions);

                return RedirectToAction("Fullfill", "Shipment",new { id = shipment.Id });
            }
            catch (MicroStoreClientException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                ex.Error.MapToModelState(ModelState);

                return RedirectToAction("Details", new {orderId = model.OrderId});
            }
        }

        private async Task<ShipmentCreateRequestOptions> PrepareShipmentCreateRequestOptions(Guid orderId)
        {
            var order = await _orderService.GetAsync(orderId);


            var requestOptions = new ShipmentCreateRequestOptions
            {
                Address = order.ShippingAddress,
                OrderId = order.Id.ToString(),
                OrderNumber = order.OrderNumber,
                UserId = order.UserId,
                Items = new List<ShipmentItemCreateRequestOptions>()
            };

            foreach (var item in order.Items)
            {
                var product = await _productService.GetAsync(item.ExternalProductId);

                requestOptions.Items.Add(new ShipmentItemCreateRequestOptions
                {
                    ProductId = item.ExternalProductId,
                    Name = item.Name,
                    Sku = item.Sku,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    Thumbnail = item.Thumbnail,
                    Dimension = product.Dimensions,
                    Weight = product.Weight
                });

            }

            return requestOptions;
        }

        [HttpPost]
        public async Task<IActionResult> CompleteOrder(CompleteOrderModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var order = await _orderService.GetAsync(model.Id);

                if (order.CurrentState != OrderState.Fullfilled)
                {
                    return BadRequest("Order State must be fullfilled");
                }

                await _orderService.CompleteAsync(model.Id);

                return RedirectToAction("Details", new { id = model.Id });

            }
            catch (MicroStoreClientException ex)
            {
                if(ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest(ex.Error);
                }

                if(ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }

                throw ex;
            }
          
        }

        public async Task<IActionResult> CompleteOrderModal(Guid id)
        {
            try
            {
                var order = await _orderService.GetAsync(id);

                if (order.CurrentState != OrderState.Fullfilled) return BadRequest();

                var ordervm = ObjectMapper.Map<Order, OrderVM>(order);

                ViewBag.Order = ordervm;

                var model = new CompleteOrderModel
                {
                    Id = order.Id
                };

                return PartialView("_CompleteOrderModal", model);
            }
            catch (MicroStoreClientException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }
        }
        public async Task<IActionResult> CancelOrderModal(Guid id)
        {
            try
            {
                var order = await _orderService.GetAsync(id);

                var ordervm = ObjectMapper.Map<Order, OrderVM>(order);

                ViewBag.Order = ordervm;

                var model = new CancelOrderModel
                {
                    Id = order.Id
                };

                return PartialView("_CancelOrderModal", model);

            }
            catch (MicroStoreClientException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<IActionResult> CancelOrder(CancelOrderModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var order = await _orderService.GetAsync(model.Id);

                if (order.CurrentState == OrderState.Submited || order.CurrentState == OrderState.Accepted)
                {
                    return BadRequest("Order cannot be cancelled yet");
                }

                var requestOptions = new OrderCancelRequestOptions
                {
                    Reason = model.Reason
                };

                await _orderService.CancelAsync(model.Id, requestOptions);

                return RedirectToAction("Details", new { orderId = model.Id });

            }
            catch (MicroStoreClientException ex) when(ex.StatusCode == HttpStatusCode.BadRequest || ex.StatusCode == HttpStatusCode.NotFound)
            {
                if (ex.StatusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest(ex.Error);
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }


  
}
