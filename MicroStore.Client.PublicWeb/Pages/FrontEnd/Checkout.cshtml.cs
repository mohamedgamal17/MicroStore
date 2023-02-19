using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroStore.Client.PublicWeb.Extensions;
using MicroStore.Client.PublicWeb.Infrastructure;
using MicroStore.Client.PublicWeb.Models;
using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Cart;
using MicroStore.ShoppingGateway.ClinetSdk.Extensions;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Billing;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Cart;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping;

namespace MicroStore.Client.PublicWeb.Pages.FrontEnd
{
    public class CheckoutModel : PageModel
    {


        private readonly BasketAggregateService _basketAggregateService;

        private readonly PaymentSystemService _paymentSystemService;

        private readonly IWorkContext _workContext;

        private readonly UserOrderService _userOrderService;

        private readonly UserPaymentRequestService _userPaymentRequestService;

        private readonly ShipmentRateService _shipmentRateService;
        private readonly ILogger<CheckoutModel> _logger;

        public CheckoutModel(BasketAggregateService basketAggregateService, PaymentSystemService paymentSystemService, IWorkContext workContext, UserOrderService userOrderService, UserPaymentRequestService userPaymentRequestService, ShipmentRateService shipmentRateService, ILogger<CheckoutModel> logger)
        {
            _basketAggregateService = basketAggregateService;
            _paymentSystemService = paymentSystemService;
            _workContext = workContext;
            _userOrderService = userOrderService;
            _userPaymentRequestService = userPaymentRequestService;
            _shipmentRateService = shipmentRateService;
            _logger = logger;
        }

        public List<PaymentSystem> PaymentSystems{ get; set; }
        public BasketAggregate Basket { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
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

            Basket = basketResponse;

            await PrepareModel();

            return Page();

        }



        public async Task<IActionResult> OnPostAsync()
        {
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
                Address = MapAddress(Input.UseAnotherShippingAddress ? Input.ShippingAddress : Input.BillingAddress),
                
            };

            var estimateRateResponse = await _shipmentRateService.EstimateAsync(rateRequestOptions);

            estimateRateResponse.ThrowIfFailure();

            double shippingCost = estimateRateResponse.Result.Items.Where(x => x.EstaimatedDays < 7).Select(x => x.Money.Value).Min();

            OrderSubmitRequestOptions submitRequestOptions = new OrderSubmitRequestOptions
            {
                BillingAddress = MapAddress(Input.BillingAddress),
                ShippingAddress = MapAddress(Input.UseAnotherShippingAddress ? Input.ShippingAddress : Input.BillingAddress),

                TaxCost = 0,
                ShippingCost = shippingCost,
                SubTotal = SubTotal,
                TotalPrice = SubTotal + shippingCost,

                Items = PrepareOrderItemReequest(Basket)

            };

            var orderResponse =   await _userOrderService.SubmitOrderAsync(submitRequestOptions);

            orderResponse.ThrowIfFailure();
            
            var order = orderResponse.Result;

            var paymentRequestOptions = new PaymentRequestOptions
            {
                OrderId = order.Id.ToString(),
                OrderNubmer = order.OrderNumber,
                ShippingCost = order.ShippingCost,
                TaxCost = order.TaxCost,
                SubtTotal = order.SubTotal,
                TotalCost = order.TotalPrice,
                Items = PreparePaymentProductCreateRequest(Basket)
            };

            var paymentResponse = await _userPaymentRequestService.CreateAsync(paymentRequestOptions);

            paymentResponse.ThrowIfFailure();


            var payment = paymentResponse.Result;

            var processPaymentRequestOptions = new PaymentProcessRequestOptions
            {
                PaymentGatewayName = "stripe_gateway",
                ReturnUrl = HttpContext.GetCurrentUrl(),
                CancelUrl = HttpContext.GetCurrentUrl()
            };

            var paymentProcessResponse = await _userPaymentRequestService.ProcessAsync(payment.Id, processPaymentRequestOptions);

            paymentProcessResponse.ThrowIfFailure();

            return Redirect(paymentProcessResponse.Result.CheckoutLink);
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
                Image = x.Thumbnail,
                Name = x.Name,
                Sku = x.Sku,
                Quantity = x.Quantity,
                UnitPrice = x.Price,


            }).ToList();
        }

        private async Task PrepareModel()
        {

            SubTotal = Basket.Items.Aggregate((double)0, (t1, t2) => t1 + (t2.Price * t2.Quantity));

            Total = Basket.Items.Aggregate((double)0, (t1, t2) => t1 + (t2.Price * t2.Quantity));

            var response = await _paymentSystemService.ListAsync();

            response.ThrowIfFailure();

            PaymentSystems = response.Result.Items;
        }
        private async Task PrepareAvailablePaymentSystems()
        {
 
        }



        public class InputModel
        {
            public AddressModel BillingAddress { get; set; }
            public bool UseAnotherShippingAddress { get; set; }
            public AddressModel ShippingAddress { get; set; }
        }
    }
}
