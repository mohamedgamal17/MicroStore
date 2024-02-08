﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStore.Bff.Shopping.Data;
using MicroStore.Bff.Shopping.Data.Billing;
using MicroStore.Bff.Shopping.Models.Billing;
using MicroStore.Bff.Shopping.Services.Billing;
namespace MicroStore.Bff.Shopping.Areas.Administration
{
    [ApiExplorerSettings(GroupName = "Administration")]
    [Route("api/administration/payments")]
    [ApiController]
    [Authorize]
    public class PaymentController : Controller
    {
        private Services.Billing.PaymentService _paymentService;
        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<Payment>))]
        public async Task<ActionResult<PagedList<Payment>>> ListPayments(string userId = "", string orderNumber = "", string status = "", double minPrice = -1, double maxPrice = -1, DateTime startDate = default, DateTime endDate = default, int skip = 0, int length = 10, string sortBy = "", bool desc = false)
        {
            var result = await _paymentService.ListAsync(userId, orderNumber, status, minPrice, maxPrice, startDate, endDate, skip, length, sortBy, desc);

            return result;
        }

        [Route("{paymentId}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Payment))]
        public async Task<ActionResult<Payment>> GetPayment(string paymentId)
        {
            var result = await _paymentService.GetAsync(paymentId);

            return result;
        }


        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Payment))]
        public async Task<ActionResult<Payment>> CreatePayment(CreatePaymentModel model)
        {
            var result = await _paymentService.CreateAsync(model);

            return CreatedAtAction(nameof(GetPayment), new { paymentId = result.Id },result);
        }

        [Route("process/{paymentId}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentProcess))]
        public async Task<ActionResult<PaymentProcess>> ProcessPayment(string paymentId, ProcessPaymentModel model)
        {
            var result = await _paymentService.ProcessAsync(paymentId, model);

            return Ok(result);
        }

        [Route("complete")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentProcess))]
        public async Task<ActionResult<Payment>> CompletePayment(CompletePaymentModel model)
        {
            var result = await _paymentService.CompleteAsync(model);
            return Ok(result);
        }

        [Route("refund/{paymentId}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentProcess))]
        public async Task<ActionResult<Payment>> RefundPayment(string paymentId)
        {
            var result = await _paymentService.RefundAsync(paymentId);

            return Ok(result);
        }

    }
}
