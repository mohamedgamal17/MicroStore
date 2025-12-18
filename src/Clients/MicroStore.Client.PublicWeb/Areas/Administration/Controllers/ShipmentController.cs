using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Ordering;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Shipments;
using MicroStore.Client.PublicWeb.Extensions;
using MicroStore.Client.PublicWeb.Security;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Geographic;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping;
using System.Data;
using System.Net;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Controllers
{
    [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser, Roles = ApplicationSecurityRoles.Admin)]
    public class ShipmentController : AdministrationController
    {
        private readonly ShipmentService _shipmentService;

        private readonly ShipmentAggregateService _shipmentAggregateService;

        private readonly OrderService _orderService;

        private readonly CountryService _countryService;
        public ShipmentController(ShipmentService shipmentService, ShipmentAggregateService shipmentAggregateService, OrderService orderService, CountryService countryService)
        {
            _shipmentService = shipmentService;
            _shipmentAggregateService = shipmentAggregateService;
            _orderService = orderService;
            _countryService = countryService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Countries = await PrepareCountryList();

            return View(new ShipmentSearchModel ());
        }

        [HttpPost]
        public async Task<IActionResult> Index(ShipmentSearchModel model) 
        {
            var pagingOptions = new ShipmentListRequestOptions
            {
                OrderNumber = model.OrderNumber,
                TrackingNumber=  model.TrackingNumber,
                Status = model.Status ,
                Country = model.Country,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Skip = model.Skip,
                Length = model.PageSize
            };

            var data = await _shipmentAggregateService.ListAsync(pagingOptions);

            var responseModel = new ShipmentListModel
            {
                Start = model.Start,
                RecordsTotal = data.TotalCount,
                Length = model.Length,
                Draw = model.Draw,
                Data = ObjectMapper.Map<List<ShipmentAggregate>, List<ShipmentAggregateVM>>(data.Items)
            };
            return Json(responseModel);
        }

        public async Task<IActionResult> Details(string id)
        {
            var shipmentAggregate = await _shipmentAggregateService.GetAsync(id);

            return View(ObjectMapper.Map<ShipmentAggregate, ShipmentAggregateVM>(shipmentAggregate));
        }

        public async Task<IActionResult> Fullfill(string id)
        {
            var shipment = await _shipmentService.GetAsync(id);

            if (shipment.Status != ShipmentStatus.Created)
            {
                return RedirectToPage("Details" , new {shipmentId = id});
            }

            return View(new ShipmentPackageModel() {Id = id }); 
        }


        [HttpPost]

        public async Task<IActionResult> Fullfill(ShipmentPackageModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);

            }
            try
            {
                var requestOptions = ObjectMapper.Map<ShipmentPackageModel, ShipmentFullfillRequestOptions>(model);

                await _shipmentService.FullfillAsync(model.Id, requestOptions);

                return RedirectToAction("PurshaseLabel" , new  { id = model.Id });

            }catch(MicroStoreClientException ex) when(ex.StatusCode == HttpStatusCode.BadRequest)
            {
                ex.Error.MapToModelState(ModelState);

                return View(model);

            }
        }


        public async Task<IActionResult> PurshaseLabel(string id)
        {

            var shipment = await _shipmentService.GetAsync(id);

            if (shipment.Status != ShipmentStatus.Fullfilled)
            {
                return RedirectToAction(nameof(Details), new { id = id });
            }
            ViewBag.Shipment = shipment;

            await PrepareShipmentRates(shipment.Id);

            return View(new PurshaseLabelModel());
        }

        [HttpPost]
        public async Task<IActionResult> PurshaseLabel(string id, PurshaseLabelModel model)
        {
            if (!ModelState.IsValid)
            {
                var shipment = await _shipmentService.GetAsync(id);

                ViewBag.Shipment = shipment;

                await PrepareShipmentRates(shipment.Id);

                return View(model);
            }

            try
            {

                var requestOptions = new PurchaseLabelRequestOptions { ShipmentRateId = model.ShipmentRateId };

                await _shipmentService.PurchaseLabelAsync(id, requestOptions);

                return RedirectToAction("Details", new {id = id });
            }
            catch (MicroStoreClientException ex) when(ex.StatusCode == HttpStatusCode.BadRequest)
            {
                var shipment = await _shipmentService.GetAsync(id);

                await PrepareShipmentOrderViewModel(shipment.OrderId);

                await PrepareShipmentRates(shipment.Id);

                ex.Error.MapToModelState(ModelState);

                return View();
            }
        }


        private async Task PrepareShipmentOrderViewModel(string orderId) 
        {

           var order = await _orderService.GetAsync(Guid.Parse(orderId));

            ViewBag.Order = ObjectMapper.Map<Order, OrderVM>(order);
        }

        private async Task PrepareShipmentRates(string shipmentId)
        {
            ViewBag.ShipmentRates = await _shipmentService.ListRatesAsync(shipmentId);
        }


        private async Task<List<SelectListItem>> PrepareCountryList()
        {
            var countries = await _countryService.ListAsync();

            var selectListItems = countries.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.TwoLetterIsoCode
            }).ToList();

            return selectListItems;
        }

    }

    public class PurshaseLabelModel
    {
        public string ShipmentRateId { get; set; }
  
    }
   
}
