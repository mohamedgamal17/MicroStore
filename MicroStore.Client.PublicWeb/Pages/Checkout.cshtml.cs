using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.AspNetCore.UI;
using MicroStore.Client.PublicWeb.Extensions;
using MicroStore.Client.PublicWeb.Models;
using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Cart;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Billing;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Cart;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping;
namespace MicroStore.Client.PublicWeb.Pages
{
    public class CheckoutModel : PageModel
    {
        [BindProperty]
        public AddressModel ShippingAddress { get; set; }

        [BindProperty]
        public bool UseAnotherBillingAddress { get; set; }

        [BindProperty]
        public AddressModel BillingAddress { get; set; }

        [BindProperty]
        public string PaymentMethod { get; set; }

        private readonly IWorkContext _workContext;

        private readonly UserOrderService _userOrderService;

        private readonly UserPaymentRequestService _userPaymentRequestService;

        private readonly ShipmentRateService _shipmentRateService;

        private BasketAggregateService _basketAggregateService;

        public CheckoutModel(IWorkContext workContext, UserOrderService userOrderService, UserPaymentRequestService userPaymentRequestService, ShipmentRateService shipmentRateService, BasketAggregateService basketAggregateService)
        {
            _workContext = workContext;
            _userOrderService = userOrderService;
            _userPaymentRequestService = userPaymentRequestService;
            _shipmentRateService = shipmentRateService;
            _basketAggregateService = basketAggregateService;
        }

        public List<PaymentSystem> PaymentSystems { get; set; }
        public BasketAggregate Basket { get; set; }
        public double SubTotal { get; set; }
        public double Total { get; set; }



        public async Task<IActionResult> OnGetAsync()
        {
            var basketResponse = await _basketAggregateService
                 .RetriveBasket(_workContext.TryToGetCurrentUserId());

            if (basketResponse.Items.Count < 1)
            {
                return Redirect("~/frontend/cart");
            }

            return Page();

        }



        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var basketResponse = await _basketAggregateService
              .RetriveBasket(_workContext.TryToGetCurrentUserId());

            if (basketResponse.Items.Count < 1)
            {
                return Redirect("~/frontend/cart");
            }

            Basket = basketResponse;

            var rateRequestOptions = new ShipmentRateEstimateRequestOptions
            {
                Items = PrepareShipmentItemEstimateRequest(Basket),

                Address = MapAddress(ShippingAddress),

            };

            var estimateRateResponse = await _shipmentRateService.EstimateAsync(rateRequestOptions);


            double shippingCost = estimateRateResponse.Where(x => x.EstaimatedDays < 7).Select(x => x.Money.Value).Min();

            double subTotal = basketResponse.Items.Sum(x => x.Price * x.Quantity);

            double total = subTotal + shippingCost;

            OrderSubmitRequestOptions submitRequestOptions = new OrderSubmitRequestOptions
            {
                BillingAddress = MapAddress(BillingAddress),

                ShippingAddress = MapAddress(ShippingAddress),

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
                ReturnUrl = HttpContext.GetCurrentUrl(),
                CancelUrl = HttpContext.GetCurrentUrl()
            };

            var paymentProcessResponse = await _userPaymentRequestService.ProcessAsync(paymentResponse.Id, processPaymentRequestOptions);

            return Redirect(paymentProcessResponse.CheckoutLink);
        }




        private Address MapAddress(AddressModel model)
        {
            return new Address
            {
                Name = string.Format("{0} {1}", model.FirstName, model.LastName),
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
