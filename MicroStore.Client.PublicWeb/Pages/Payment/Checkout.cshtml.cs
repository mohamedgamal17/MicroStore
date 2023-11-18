using CookieManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.AspNetCore.UI;
using MicroStore.Client.PublicWeb.Consts;
using MicroStore.Client.PublicWeb.Extensions;
using MicroStore.Client.PublicWeb.Infrastructure;
using MicroStore.Client.PublicWeb.Models;
using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Cart;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Profiling;
using MicroStore.ShoppingGateway.ClinetSdk.Exceptions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Billing;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Cart;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping;

namespace MicroStore.Client.PublicWeb.Pages.Payment
{
    [Authorize]
    [CheckProfilePageCompletedFilter]
    public class CheckoutModel : PageModel
    {
        [BindProperty]
        public string PaymentMethod { get; set; }

        [BindProperty]
        public string BillingAddressId { get; set; }

        [BindProperty]

        public string ShippingAddressId { get; set; }

        public User Profile { get; set; }



        private readonly IWorkContext _workContext;

        private readonly UserOrderService _userOrderService;

        private readonly UserPaymentRequestService _userPaymentRequestService;

        private readonly ShipmentRateService _shipmentRateService;

        private BasketAggregateService _basketAggregateService;

        private readonly UINotificationManager _notificationManager;

        private readonly UserProfileService _profileService;

        private readonly ICookieManager _cookieManager;

        public CheckoutModel(IWorkContext workContext, UserOrderService userOrderService, UserPaymentRequestService userPaymentRequestService, ShipmentRateService shipmentRateService, BasketAggregateService basketAggregateService, UINotificationManager notificationManager, UserProfileService profileService, ICookieManager cookieManager)
        {
            _workContext = workContext;
            _userOrderService = userOrderService;
            _userPaymentRequestService = userPaymentRequestService;
            _shipmentRateService = shipmentRateService;
            _basketAggregateService = basketAggregateService;
            _notificationManager = notificationManager;
            _profileService = profileService;
            _cookieManager = cookieManager;
        }
        public List<PaymentSystem> PaymentSystems { get; set; }
        public BasketAggregate Basket { get; set; }
        public double SubTotal { get; set; }
        public double Total { get; set; }


        public override async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            Basket = await _basketAggregateService
                 .RetriveBasket(_workContext.TryToGetCurrentUserId());

            if (Basket.Items.Count < 1)
            {
                _notificationManager.Error("Your cart is empty.");

                context.Result = RedirectToPage("MyCart");
            }


            try
            {
                Profile = await _profileService.GetAsync();

                await next();

                bool hasAddresses = Profile.Addresses?.Count > 0;

                if (!hasAddresses)
                {
                    context.Result = RedirectToPage("/Profile/Address/Create");
                }

            }
            catch (MicroStoreClientException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {

                _notificationManager.Error("Please complete your profile first");

                context.Result = RedirectToPage("/Profile/Create", new { returnUrl = context.HttpContext.Request.Path });
            }


        }

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var billingAddress = Profile.Addresses.SingleOrDefault(x => x.Id == BillingAddressId);

            var shippingAddress = Profile.Addresses.SingleOrDefault(x => x.Id == ShippingAddressId);

            if (billingAddress == null)
            {
                _notificationManager.Error("Invalid billing address id");

                return Page();
            }

            if (shippingAddress == null)
            {
                _notificationManager.Error("Invalid billing address id");

                return Page();
            }

            var rateRequestOptions = new ShipmentRateEstimateRequestOptions
            {
                Items = PrepareShipmentItemEstimateRequest(Basket),

                Address = billingAddress,

            };

            var estimateRateResponse = await _shipmentRateService.EstimateAsync(rateRequestOptions);

            double shippingCost = estimateRateResponse.Where(x => x.EstaimatedDays < 7).Select(x => x.Money.Value).Min();

            double subTotal = Basket.Items.Sum(x => x.Price * x.Quantity);

            double total = subTotal + shippingCost;

            OrderSubmitRequestOptions submitRequestOptions = new OrderSubmitRequestOptions
            {
                BillingAddress = billingAddress,

                ShippingAddress = shippingAddress,

                TaxCost = 0,

                ShippingCost = shippingCost,

                SubTotal = subTotal,

                TotalPrice = total,

                Items = PrepareOrderItemReequest(Basket)

            };

            var orderResponse = await _userOrderService.SubmitOrderAsync(submitRequestOptions);

            var paymentRequestOptions = new PaymentRequestOptions
            {
                OrderId = orderResponse.Id.ToString(),
                OrderNumber = orderResponse.OrderNumber,
                ShippingCost = orderResponse.ShippingCost,
                TaxCost = orderResponse.TaxCost,
                SubTotal = orderResponse.SubTotal,
                TotalCost = orderResponse.TotalPrice,
                Items = PreparePaymentProductCreateRequest(Basket)
            };

            var paymentResponse = await _userPaymentRequestService.CreateAsync(paymentRequestOptions);

            var processPaymentRequestOptions = new PaymentProcessRequestOptions
            {
                GatewayName = PaymentMethod,
                ReturnUrl = $"{HttpContext.GetHostUrl()}{Url.Page("/Payment/Complete")}",
                CancelUrl = HttpContext.GetHostUrl()
            };

            var paymentProcessResponse = await _userPaymentRequestService.ProcessAsync(paymentResponse.Id, processPaymentRequestOptions);

            var PaymentSessionModel = new PaymentSessionModel
            {
                OrderId = orderResponse.Id,
                OrderNumber = orderResponse.OrderNumber,
                PaymentRequestId = paymentResponse.Id,
                Provider = paymentProcessResponse.Provider,
                SessionId = paymentProcessResponse.SessionId,
                CheckoutUrl = paymentProcessResponse.CheckoutLink
            };

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddHours(1)
            };

            _cookieManager.Set(PaymentConsts.Cookie, PaymentSessionModel, cookieOptions);

            return Redirect(paymentProcessResponse.CheckoutLink);
        }

        private Address MapAddress(AddressModel model)
        {
            return new Address
            {
                Name = model.Name,
                CountryCode = model.Country,
                State = model.StateProvince,
                City = model.City,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                PostalCode = model.PostalCode,
                Phone = model.PhoneNumber,
                Zip = model.ZipCode
            };
        }

        private List<ShipmentItemEstimateRequestOptions> PrepareShipmentItemEstimateRequest(BasketAggregate basketAggregate)
        {
            return basketAggregate.Items.Select(x => new ShipmentItemEstimateRequestOptions
            {
                Name = x.Name,
                Sku = x.Sku,
                Quantity = x.Quantity,
                UnitPrice = new Money
                {
                    Currency = "usd",
                    Value = x.Price,
                },
                Weight = x.Weight

            }).ToList();
        }

        private List<OrderItemRequestOptions> PrepareOrderItemReequest(BasketAggregate basketAggregate)
        {
            return basketAggregate.Items.Select(x => new OrderItemRequestOptions
            {
                ExternalProductId = x.ProductId,
                Thumbnail = x.Thumbnail,
                Name = x.Name,
                Sku = x.Sku,
                Quantity = x.Quantity,
                UnitPrice = x.Price,


            }).ToList();
        }


        private List<PaymentProductCreateRequestOptions> PreparePaymentProductCreateRequest(BasketAggregate basketAggregate)
        {
            return basketAggregate.Items.Select(x => new PaymentProductCreateRequestOptions
            {

                ProductId = x.ProductId,
                Image = x.Thumbnail ?? Guid.NewGuid().ToString(), // For now we will add fallback image
                Name = x.Name,
                Sku = x.Sku,
                Quantity = x.Quantity,
                UnitPrice = x.Price,


            }).ToList();
        }
    }
}
