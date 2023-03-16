using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping;
using NUglify.Helpers;
using System.Net;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Pages.Orders
{
    public class DetailModel : PageModel
    {
        public OrderAggregate Order { get; set; }

        private readonly OrderAggregateService _orderAggregateService;

        private readonly OrderService _orderService;

        private readonly ProductService _productService;

        private readonly ShipmentService _shipmentService;
        public DetailModel(OrderAggregateService orderAggregateService, OrderService orderService, ProductService productService, ShipmentService shipmentService)
        {
            _orderAggregateService = orderAggregateService;
            _orderService = orderService;
            _productService = productService;
            _shipmentService = shipmentService;
        }

        public async Task<IActionResult> OnGet(Guid orderId)
        {
            Order = await _orderAggregateService.GetAsync(orderId);

            return Page();
        }

        public async Task<IActionResult> OnPostCreateShipment(Guid orderId)
        {
            try
            {

                var requestOptions = await PrepareShipmentCreateRequestOptions(orderId);

                var shipment = await _shipmentService.CreateAsync(requestOptions);

                return RedirectToPage("Shipments/Fullfill", new { shipmentId = shipment.Id });
            }
            catch(MicroStoreClientException ex) when(ex.StatusCode == HttpStatusCode.BadRequest)
            {
                if (ex.Erorr.Title != null) ModelState.AddModelError("", ex.Erorr.Title);
                if (ex.Erorr.Type != null) ModelState.AddModelError("", ex.Erorr.Type);
                if (ex.Erorr.Detail != null) ModelState.AddModelError("", ex.Erorr.Detail);
                if (ex.Erorr.Errors != null)
                {
                    ex.Erorr.Errors.ForEach(error => ModelState.AddModelError("", string.Format("{0}:{1}", error.Key, error.Value.JoinAsString(" , "))));
                }

                return Page();
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

            foreach(var item in order.Items)
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
