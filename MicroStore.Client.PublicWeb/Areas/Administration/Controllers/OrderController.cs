using Microsoft.AspNetCore.Mvc;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Products;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Ordering;
using MicroStore.Client.PublicWeb.Extensions;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping;
using System.Net;
using Volo.Abp.ObjectMapping;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Controllers
{
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
            return View(new OrderListModel());
        }


        [HttpPost]
        public async Task<IActionResult> Index(OrderListModel model)
        {
            var pagingOptions = new PagingAndSortingRequestOptions
            {
                Lenght = model.PageSize,
                Skip = model.PageNumber,
            };

            var data = await _orderService.ListAsync(pagingOptions);

            model.Data = ObjectMapper.Map<List<Order>, List<OrderVM>>(data.Items);

            model.RecordsTotal = data.TotalCount;

            return Json(model);
        }

        public async Task<IActionResult> Details(Guid orderId)
        {
            var orderAggregate = await _orderAggregateService.GetAsync(orderId);

            return View(ObjectMapper.Map<OrderAggregate, OrderAggregateVM>(orderAggregate));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderShipment(Guid orderId)
        {

            try
            {

                var requestOptions = await PrepareShipmentCreateRequestOptions(orderId);

                var shipment = await _shipmentService.CreateAsync(requestOptions);

                return RedirectToAction("Fullfill", "Shipment",new { shipmentId = shipment.Id });
            }
            catch (MicroStoreClientException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                ex.Erorr.MapToModelState(ModelState);

                return RedirectToAction("Details", new {orderId = orderId});
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
                UserName = order.UserId,
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
    }


  
}
