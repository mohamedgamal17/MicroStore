using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.Bff.Shopping.Data;
using MicroStore.Bff.Shopping.Data.Billing;
using MicroStore.Bff.Shopping.Infrastructure;
using MicroStore.Bff.Shopping.Models.Billing;
using MicroStore.Bff.Shopping.Services.Billing;
namespace MicroStore.Bff.Shopping.Areas.FrontEnd
{
    [ApiExplorerSettings(GroupName = "FrontEnd")]
    [Route("api/frontend/user/payments")]
    [ApiController]
    [Authorize]
    public class UserPaymentController : Controller
    {
        private readonly Services.Billing.PaymentService _paymentService;

        private readonly IWorkContext _workContext;
        public UserPaymentController(PaymentService paymentService, IWorkContext workContext)
        {
            _paymentService = paymentService;
            _workContext = workContext;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<Payment>))]
        public async Task<ActionResult<PagedList<Payment>>> ListUserPayments(string? orderNumber = null, string? status = null, double minPrice = -1, double maxPrice = -1, DateTime? startDate = null, DateTime? endDate = null, int skip = 0, int length = 10, string? sortBy = null, bool desc = false)
        {
            string userId = _workContext.User!.Id;

            var result = await _paymentService.ListAsync(userId, orderNumber, status, minPrice, maxPrice, startDate, endDate, skip, length, sortBy, desc);

            return result;
        }

        [Route("{paymentId}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Payment))]
        public async Task<ActionResult<Payment>> GetUserPayment(string paymentId)
        {
            var result = await _paymentService.GetAsync(paymentId);

            return result;
        }


        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Payment))]
        public async Task<IActionResult> CreatePayment(PaymentModel model)
        {
            string userId = _workContext.User!.Id;

            var createPaymentModel = (CreatePaymentModel)model;

            createPaymentModel.UserId = userId;

            var result = await _paymentService.CreateAsync(createPaymentModel);

            return CreatedAtAction(nameof(GetUserPayment), new { paymentId = result.Id });
        }

        [Route("process/{paymentId}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Payment))]
        public async Task<ActionResult<PaymentProcess>> ProcessPayment(string paymentId,ProcessPaymentModel model)
        {
            var result = await _paymentService.ProcessAsync(paymentId, model);

            return result;
        }

        [Route("complete")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Payment))]
        public async Task<ActionResult<Payment>> CompletePayment(CompletePaymentModel model)
        {
            var result = await _paymentService.CompleteAsync(model);

            return result;
        }

    }
}
