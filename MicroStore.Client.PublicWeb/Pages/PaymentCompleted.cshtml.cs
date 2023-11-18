using CookieManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.AspNetCore.UI;
using MicroStore.Client.PublicWeb.Consts;
using MicroStore.Client.PublicWeb.Models;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Billing;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Cart;

namespace MicroStore.Client.PublicWeb.Pages
{
    public class PaymentCompletedModel : PageModel
    {

        private readonly ICookieManager _cookieManager;
        private readonly UserPaymentRequestService _userPaymentRequestService;
        private readonly BasketService _basketService;
        private readonly IWorkContext _workContext;
        public bool HasError { get; set; }
        public string Error { get; set; }
        public PaymentCompletedModel(ICookieManager cookieManager, UserPaymentRequestService userPaymentRequestService, BasketService basketService, IWorkContext workContext)
        {
            _cookieManager = cookieManager;
            _userPaymentRequestService = userPaymentRequestService;
            _basketService = basketService;
            _workContext = workContext;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var paymentSessionModel = _cookieManager.Get<PaymentSessionModel>(PaymentConsts.Cookie);

            if (paymentSessionModel == null)
            {
                return NotFound();
            }

            if (paymentSessionModel.Status == PaymentSessionStatus.Completed)
            {
                return RedirectToPage("Orders/Recived", new { orderNumber = paymentSessionModel.OrderNumber });
            }
            else
            {
                try
                {
                    var requestOptions = new PaymentCompleteRequestOptions
                    {
                        GatewayName = paymentSessionModel.Provider,
                        SessionId = paymentSessionModel.SessionId
                    };

                    var paymentRequest = await _userPaymentRequestService.CompleteAsync(requestOptions);

                    var userId = _workContext.TryToGetCurrentUserId();

                    await _basketService.ClearAsync(userId);

                    paymentSessionModel.Status = PaymentSessionStatus.Completed;

                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.UtcNow.AddHours(1),
                    };

                    _cookieManager.Set(PaymentConsts.Cookie, paymentSessionModel, cookieOptions);

                    return RedirectToPage("Orders/Recived", new { orderNumber = paymentRequest.OrderNumber });
                }
                catch (MicroStoreClientException ex) when (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    HasError = true;
                    Error = ex.Erorr.Detail;


                    return Page();
                }
            }

        

           
        }
    }
}
