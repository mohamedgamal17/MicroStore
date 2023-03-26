using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Ordering;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Shipments;
using MicroStore.Client.PublicWeb.Extensions;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping;
using System.Net;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Controllers
{
    public class ShipmentController : AdministrationController
    {
        private readonly ShipmentService _shipmentService;

        private readonly ShipmentAggregateService _shipmentAggregateService;

        private readonly OrderService _orderService;

        public ShipmentController(ShipmentService shipmentService, ShipmentAggregateService shipmentAggregateService, OrderService orderService)
        {
            _shipmentService = shipmentService;
            _shipmentAggregateService = shipmentAggregateService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            return View(new ShipmentListModel ());
        }

        [HttpPost]
        public async Task<IActionResult> Index(ShipmentListModel model) 
        {
            var pagingOptions = new PagingReqeustOptions
            {
                Skip = model.PageNumber,
                Lenght = model.PageSize
            };

            var data = await _shipmentService.ListAsync(pagingOptions);

            model.Data = ObjectMapper.Map<List<Shipment>, List<ShipmentVM>>(data.Items);

            model.RecordsTotal = data.TotalCount;

            return Json(model);
        }

        public async Task<IActionResult> Details(string shipmentId)
        {
            var shipmentAggregate = await _shipmentAggregateService.GetAsync(shipmentId);

            return View(ObjectMapper.Map<ShipmentAggregate, ShipmentAggregateVM>(shipmentAggregate));
        }




        public async Task<IActionResult> Fullfill(string shipmentId)
        {
            var shipment = await _shipmentService.GetAsync(shipmentId);

            if (shipment.Status != ShipmentStatus.Created)
            {
                return RedirectToPage("Details" , new {shipmentId = shipmentId});
            }

            return View(new ShipmentPackageModel() {ShipmentId = shipmentId }); 
        }


        [HttpPost]

        public async Task<IActionResult> Fullfill(string shipmentId , ShipmentPackageModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);

            }
            try
            {
                var requestOptions = ObjectMapper.Map<ShipmentPackageModel, ShipmentFullfillRequestOptions>(model);

                await _shipmentService.FullfillAsync(shipmentId, requestOptions);

                return RedirectToAction("PurshaseLabel" , new  { shipmentId = shipmentId });

            }catch(MicroStoreClientException ex) when(ex.StatusCode == HttpStatusCode.BadRequest)
            {
                ex.Erorr.MapToModelState(ModelState);

                return View(model);

            }
        }


        public async Task<IActionResult> PurshaseLabel(string shipmentId)
        {

            var shipment = await _shipmentService.GetAsync(shipmentId);

            if (shipment.Status != ShipmentStatus.Fullfilled)
            {
                return RedirectToPage("Details", new { shipmentId = shipmentId });
            }

            await PrepareShipmentOrderViewModel(shipment.Id);

            await PrepareShipmentRates(shipment.Id);

            return View(new PurshaseLabelModel { ShipmentId = shipmentId});
        }

        [HttpPost]
        public async Task<IActionResult> PurshaseLabel(string shipmentId, PurshaseLabelModel model)
        {
            if (!ModelState.IsValid)
            {
                var shipment = await _shipmentService.GetAsync(shipmentId);

                await PrepareShipmentOrderViewModel(shipment.Id);

                await PrepareShipmentRates(shipment.Id);

                return View(model);
            }

            try
            {

                var requestOptions = new PurchaseLabelRequestOptions { ShipmentRateId = model.ShipmentRateId };

                await _shipmentService.PurchaseLabelAsync(shipmentId, requestOptions);

                return RedirectToAction("Details", new {shipmentId = shipmentId });
            }
            catch (MicroStoreClientException ex) when(ex.StatusCode == HttpStatusCode.BadRequest)
            {
                var shipment = await _shipmentService.GetAsync(shipmentId);

                await PrepareShipmentOrderViewModel(shipment.Id);

                await PrepareShipmentRates(shipment.Id);

                ex.Erorr.MapToModelState(ModelState);

                return View();
            }
        }


        private async Task PrepareShipmentOrderViewModel( string orderId) 
        {

           var order = await _orderService.GetAsync(Guid.Parse(orderId));

            ViewBag.Order = ObjectMapper.Map<Order, OrderVM>(order);
        }

        private async Task PrepareShipmentRates(string shipmentId)
        {
            ViewBag.ShipmentRates = await _shipmentService.RetrieveRatesAsync(shipmentId);
        }



    }

    public class PurshaseLabelModel
    {
        [BindNever]
        public string ShipmentId { get; set; }
        public string ShipmentRateId { get; set; }
  
    }
   
}
