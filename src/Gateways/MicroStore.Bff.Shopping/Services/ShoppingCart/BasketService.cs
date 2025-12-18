using MicroStore.Bff.Shopping.Data.Billing;
using MicroStore.Bff.Shopping.Data.Catalog;
using MicroStore.Bff.Shopping.Data.Ordering;
using MicroStore.Bff.Shopping.Data.Profiling;
using MicroStore.Bff.Shopping.Data.Shipping;
using MicroStore.Bff.Shopping.Data.ShoppingCart;
using MicroStore.Bff.Shopping.Exceptions;
using MicroStore.Bff.Shopping.Grpc.Profiling;
using MicroStore.Bff.Shopping.Grpc.ShoppingCart;
using MicroStore.Bff.Shopping.Models.Billing;
using MicroStore.Bff.Shopping.Models.Common;
using MicroStore.Bff.Shopping.Models.Ordering;
using MicroStore.Bff.Shopping.Models.Shipping;
using MicroStore.Bff.Shopping.Models.ShoppingCart;
using MicroStore.Bff.Shopping.Services.Billing;
using MicroStore.Bff.Shopping.Services.Catalog;
using MicroStore.Bff.Shopping.Services.Ordering;
using MicroStore.Bff.Shopping.Services.Profiling;
using MicroStore.Bff.Shopping.Services.Shipping;

namespace MicroStore.Bff.Shopping.Services.ShoppingCart
{
    public class BasketService
    {
        private readonly Grpc.ShoppingCart.BasketService.BasketServiceClient _basketService;
        private readonly ProductService _productService;
        private readonly AddressService _addressService;
        private readonly ProfilingService _profileService;
        private readonly OrderService _orderService;
        private readonly PaymentService _paymentService;
        private readonly RateService _rateService;
        public BasketService(Grpc.ShoppingCart.BasketService.BasketServiceClient basketService, ProductService productService, AddressService addressService, ProfilingService profileService)
        {
            _basketService = basketService;
            _productService = productService;
            _addressService = addressService;
            _profileService = profileService;
        }

        public async Task<Basket> GetAsync(string userId, CancellationToken cancellationToken = default)
        {
            var request = new GetByUserIdRequest { UserId = userId };

            var basket = await _basketService.GetByUserIdAsync(request);

            var productIds = basket.Items?.Select(x => x.ProductId).ToList() ?? new List<string>();

            List<Product> products = new List<Product>();


            if(productIds.Count > 0)
            {
                products = await _productService.ListByIdsAsync(productIds);
            }

            return PrepareBasket(basket, products);
        }

        public async Task<Basket> AddOrUpdateItemAsync(string userId ,BasketItemModel model ,CancellationToken cancellationToken = default)
        {
            var request = new AddOrUpdateBasketItemRequest
            {
                UserId = userId,
                ProductId = model.ProductId,
                Quantity = model.Quantity
            };

            var response = await _basketService.AddOrUpdateItemAsync(request);

            var productIds = response.Items?.Select(x => x.ProductId).ToList() ?? new List<string>();

            List<Product> products = new List<Product>();

            if (productIds.Count > 0)
            {
                products = await _productService.ListByIdsAsync(productIds);
            }

            return PrepareBasket(response, products);

        }


        public async Task<Basket> RemoveItemAsync(string userId , RemoveBasketItemModel model , CancellationToken  cancellationToken= default)
        {
            var request = new RemoveBasketItemRequest { ProductId = model.ProductId , UserId = userId};

            var response = await _basketService.RemoveProductAsync(request);

            var productIds = response.Items?.Select(x => x.ProductId).ToList() ?? new List<string>();

            List<Product> products = new List<Product>();

            if (productIds.Count > 0)
            {
                products = await _productService.ListByIdsAsync(productIds);
            }

            return PrepareBasket(response, products);
        }


        public async Task<Basket> MigrateAsync(BasketMigrationModel model,  CancellationToken cancellationToken = default)
        {
            var request = new MigrateRequest
            {
                FromUserId = model.FromUserId,
                ToUserId = model.ToUserId
            };

            var response = await _basketService.MigrateAsync(request);

            var productIds = response.Items?.Select(x => x.ProductId).ToList() ?? new List<string>();

            List<Product> products = new List<Product>();

            if (productIds.Count > 0)
            {
                products = await _productService.ListByIdsAsync(productIds);
            }

            return PrepareBasket(response, products);
        }

        public async Task Clear(string userId , CancellationToken cancellationToken = default)
        {
            var request = new CleareUserBasketReqeust { UserId = userId };

            var response = await _basketService.ClearAsync(request);


        }

        public async Task<PaymentProcess> Checkout(string userId , CheckoutModel model , CancellationToken cancellationToken  = default)
        {
            var basket = await GetAsync(userId);

            if(basket.Items.Count <= 0)
            {
                throw new BusinessLogicException("User basket is empty . cannot complete this operation");
            }

            var shippingAddressTask = _profileService.GetUserAddressAsync(userId, model.ShippingAddressId);
            var billingAddressTask = _profileService.GetUserAddressAsync(userId, model.BillingAddressId);

            await Task.WhenAll(shippingAddressTask, billingAddressTask);

            var estimateRateModel = PrepareEstimateRateModel(basket, shippingAddressTask.Result);

            var rates = await _rateService.EstimateAsync(estimateRateModel);

            var orderModel = PrepareOrderModel(userId, basket, rates, shippingAddressTask.Result, billingAddressTask.Result);

            var order = await _orderService.CreateAsync(orderModel);

            var paymentModel = PreparePaymentModel(order);

            var payment = await _paymentService.CreateAsync(paymentModel);

            var paymentProcess = await _paymentService.ProcessAsync(payment.Id, new ProcessPaymentModel
            {
                CancelUrl = model.CancelUrl,
                ReturnUrl = model.ReturnUrl,
                GatewayName = model.PaymentMethod
            });

            return paymentProcess;
        }


        private EstimateRateModel PrepareEstimateRateModel(Basket basket , Address shippingAddress)
        {

            var model = new EstimateRateModel
            {
                Address = new AddressModel
                {
                    Name = shippingAddress.Name,
                    Country = shippingAddress.Country.TwoIsoCode,
                    State = shippingAddress.State.Abbreviation,
                    City = shippingAddress.City,
                    AddressLine1 = shippingAddress.AddressLine1,
                    AddressLine2 = shippingAddress.AddressLine2,
                    PostalCode = shippingAddress.PostalCode,
                    Phone = shippingAddress.Phone,
                    Zip = shippingAddress.Zip
                },
                Items = basket.Items.Select(x => new EstimatedRateItemModel
                {
                    Name = x.Product.Name,
                    Sku = x.Product.Sku,
                    Quantity = x.Quantity,
                    UnitPrice = new Data.Common.Money
                    {
                        Currency = "usd",
                        Value = x.Product.Price
                    },
                    Weight = new WeightModel
                    {
                        Value = x.Product.Weight.Value,
                        Unit = x.Product.Weight.Unit
                    }
                }).ToList()
            };

            return model;
        }
        private OrderModel PrepareOrderModel(string userId,Basket basket, List<Rate> shippingRates ,Address shippingAddress,  Address billingAddress)
        {
            double shippingCost = shippingRates.Where(x => x.EstimatedDays < 7).Select(x => x.Money.Value).Min();

            double subTotal = basket.Items.Sum(x => x.Product.Price * x.Quantity);

            double total = shippingCost + subTotal;

            var model = new OrderModel
            {
                UserId = userId,
                TaxCost = 0,
                ShippingCost = shippingCost,
                SubTotal = subTotal,
                TotalPrice = total,
                ShippingAddress = new AddressModel
                {
                    Name = shippingAddress.Name,
                    Country = shippingAddress.Country.TwoIsoCode,
                    State = shippingAddress.State.Abbreviation,
                    City = shippingAddress.City,
                    AddressLine1 = shippingAddress.AddressLine1,
                    AddressLine2 = shippingAddress.AddressLine2,
                    PostalCode = shippingAddress.PostalCode,
                    Phone = shippingAddress.Phone,
                    Zip = shippingAddress.Zip
                },
                BillingAddress = new AddressModel
                {
                    Name = billingAddress.Name,
                    Country = billingAddress.Country.TwoIsoCode,
                    State = billingAddress.State.Abbreviation,
                    City = billingAddress.City,
                    AddressLine1 = billingAddress.AddressLine1,
                    AddressLine2 = billingAddress.AddressLine2,
                    PostalCode = billingAddress.PostalCode,
                    Phone = billingAddress.Phone,
                    Zip = billingAddress.Zip
                },
                Items = basket.Items.Select(x => new OrderItemModel
                {
                    Name = x.Product.Name,
                    Sku = x.Product.Sku,
                    ProductId = x.Product.Id,
                    Quantity = x.Quantity,
                    UnitPrice = x.Product.Price,
                    Thumbnail = x.Product.Images.FirstOrDefault()?.Image ?? string.Empty
                }).ToList()
            };
            return model;
        }

        private CreatePaymentModel PreparePaymentModel(Order order)
        {
            var model = new CreatePaymentModel
            {
                UserId = order.User.Id,
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                ShippingCost = order.ShippingCost,
                SubTotal = order.SubTotal,
                TaxCost = order.TaxCost,
                TotalCost = order.TotalPrice,
                Items = order.Items.Select(x => new PaymentItemModel
                {
                    Name = x.Name,
                    Sku = x.Sku,
                    Image = x.Thumbnail,
                    ProductId = x.ExternalProductId,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice
                }).ToList()
            };

            return model;
        }

        private Basket PrepareBasket(BasketResponse response,  List<Product> products)
        {
            var basket = new Basket
            {
                Id = response.UserId,
                UserId = response.UserId,
                Items =new List<BasketItem>()
            };


            foreach (var item in response.Items)
            {
                var basketItem = new BasketItem
                {
                    Id = item.ProductId,
                    ProductId = item.ProductId,
                    Product = products.SingleOrDefault(x => x.Id == item.ProductId),
                    Quantity = item.Quantity
                };

                basket.Items.Add(basketItem);
            }


            return basket;
        }

    }
}
