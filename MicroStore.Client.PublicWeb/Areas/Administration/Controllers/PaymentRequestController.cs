using Microsoft.AspNetCore.Mvc;
using MicroStore.BuildingBlocks.AspNetCore.Models;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Billing;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Products;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Services;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Billing;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Controllers
{
    public class PaymentRequestController : AdministrationController
    {
        private readonly PaymentRequestService _paymentRequestService;
        public PaymentRequestController(PaymentRequestService paymentRequestService)
        {
            _paymentRequestService = paymentRequestService;
        }


        public async Task<IActionResult> Index()
        {
        
            return View(new PaymentRequestListModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(PaymentRequestListModel model)
        {
            var pagingOptions = new PagingReqeustOptions
            {
                Skip = model.PageNumber,
                Lenght = model.PageSize,
            };

            var data = await _paymentRequestService.ListAsync(pagingOptions);

            model.Data = ObjectMapper.Map<List<PaymentRequest>, List<PaymentRequestVM>>(data.Items);

            model.RecordsTotal = data.TotalCount;

            return Json(model);
        }


        public async Task<IActionResult> Details(string paymentRequestId)
        {
            var payments = await _paymentRequestService.GetAsync(paymentRequestId);

            return View(ObjectMapper.Map<PaymentRequest, PaymentRequestVM>(payments));
        }
    }
}
