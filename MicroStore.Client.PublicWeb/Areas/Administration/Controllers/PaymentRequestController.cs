using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Billing;
using MicroStore.Client.PublicWeb.Security;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Billing;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Controllers
{
    [Authorize(Policy = ApplicationSecurityPolicies.RequireAuthenticatedUser, Roles = ApplicationSecurityRoles.Admin)]
    public class PaymentRequestController : AdministrationController
    {
        private readonly PaymentRequestService _paymentRequestService;
        public PaymentRequestController(PaymentRequestService paymentRequestService)
        {
            _paymentRequestService = paymentRequestService;
        }
        public async Task<IActionResult> Index()
        {       
            return View(new PaymentRequestSearchModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(PaymentRequestSearchModel model)
        {
            var pagingOptions = new PaymentListRequestOptions
            {
                OrderNumber = model.OrderNumber,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                MinPrice = model.MinPrice,
                MaxPrice = model.MaxPrice,
                Status = model.Status,
                Skip = model.PageNumber,
                Length = model.PageSize,
            };

            var data = await _paymentRequestService.ListAsync(pagingOptions);

            var responseModel = new PaymentRequestListModel
            {
                Start = model.Start,
                RecordsTotal = model.RecordsTotal,
                Length = model.Length,
                Draw = model.Draw,
                Data = ObjectMapper.Map<List<PaymentRequest>, List<PaymentRequestVM>>(data.Items),
            };

            return Json(responseModel);
        }


        public async Task<IActionResult> Details(string id)
        {
            var payments = await _paymentRequestService.GetAsync(id);

            return View(ObjectMapper.Map<PaymentRequest, PaymentRequestVM>(payments));
        }
    }
}
