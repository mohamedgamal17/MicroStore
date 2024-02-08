using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MicroStore.Bff.Shopping.Data;
using MicroStore.Bff.Shopping.Data.Billing;
using MicroStore.Bff.Shopping.Data.Common;
using MicroStore.Bff.Shopping.Data.Geographic;
using MicroStore.Bff.Shopping.Data.Ordering;
using MicroStore.Bff.Shopping.Data.Profiling;
using MicroStore.Bff.Shopping.Data.Shipping;
using MicroStore.Bff.Shopping.Grpc.Ordering;
using MicroStore.Bff.Shopping.Grpc.Shipping;
using MicroStore.Bff.Shopping.Models.Ordering;
namespace MicroStore.Bff.Shopping.Services.Ordering
{
    public class OrderService
    {
        private readonly Grpc.Ordering.OrderService.OrderServiceClient _orderServiceClient;

        private readonly Profiling.ProfilingService _profilingService;

        private readonly Geographic.CountryService _countryService;

        private readonly Billing.PaymentService _paymentService;

        private readonly Shipping.ShipmentService _shipmentService;

        public OrderService(Grpc.Ordering.OrderService.OrderServiceClient orderServiceClient, Profiling.ProfilingService profilingService, Geographic.CountryService countryService, Billing.PaymentService paymentService, Shipping.ShipmentService shipmentService)
        {
            _orderServiceClient = orderServiceClient;
            _profilingService = profilingService;
            _countryService = countryService;
            _paymentService = paymentService;
            _shipmentService = shipmentService;
        }

        public async Task<Order> CreateAsync(OrderModel model , CancellationToken cancellationToken = default)
        {
            var request = PrepareCreateOrderRequest(model);

            var response = await _orderServiceClient.CreateAsync(request);

            return await PrepareSingleOrder(response);
        }

        public async Task FullfillAsync(string orderId, FullfillModel model ,CancellationToken cancellationToken = default)
        {
            var request = new FullfillOrderRequest { OrderId = orderId, ShipmentId = model.ShipmentId };

            var response = await _orderServiceClient.FullfillAsync(request);
        }

        public async Task CompleteAsync(string orderId,  CompleteOrderModel model ,  CancellationToken cancellationToken = default)
        {
            var request = new CompleteOrderReqeust { OrderId = orderId };

            var response = await _orderServiceClient.CompleteAsync(request);
        }

        public async Task CancelAsync(string orderId,  CancelOrderModel model , CancellationToken cancellationToken)
        {
            var request = new CancelOrderReqeust
            {
                OrderId = orderId,
                Reason = model.Reason
            };

            var response = await _orderServiceClient.CancelAsync(request);
        }

        public async Task<PagedList<Order>> ListAsync(string? userId = null,string? orderNumber = null, string? states = null, DateTime? startDate =null, DateTime? endDate = null, int skip =0 , int length =10, string? sortBy = null, bool desc = false,CancellationToken cancellationToken = default)
        {
            var request = new OrderListReqeust
            {
                UserId = userId,
                OrderNumber = orderNumber,
                States = states,
                StartSubmissionDate = startDate?.ToUniversalTime().ToTimestamp(),
                EndSubmissionDate = endDate?.ToUniversalTime().ToTimestamp(),
                Skip = skip,
                Length = length,
                SortBy = sortBy,
                Desc = desc
            };

            var response = await _orderServiceClient.GetListAsync(request);

            var paged = new PagedList<Order>
            {
                Length = response.Length,
                Skip = response.Skip,
                TotalCount = response.TotalCount,
                Items = await PrepareOrderListResponse(response.Items)
            };

            return paged;
        }

        public async Task<Order> GetAsync(string orderId, CancellationToken cancellationToken = default)
        {
            var request = new GetOrderByIdReqeuest { OrderId = orderId };

            var response = await _orderServiceClient.GetbyIdAsync(request);

            return await PrepareSingleOrder(response);
        }

        public async Task<Order> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
        {
            var request = new GetOrderByNumberRequest { OrderNumber = orderNumber };

            var response = await _orderServiceClient.GetByNumberAsync(request);

            return await PrepareSingleOrder(response);
        }

        private CreateOrderRequest PrepareCreateOrderRequest(OrderModel model)
        {
            var request = new CreateOrderRequest
            {
                UserId = model.UserId,
                ShippingCost = model.ShippingCost,
                SubTotal = model.SubTotal,
                TaxCost = model.TaxCost,
                TotalPrice = model.TotalPrice,
                ShippingAddress = new Grpc.Ordering.Address
                {
                    Name = model.ShippingAddress.Name,
                    CountryCode = model.ShippingAddress.Country,
                    City = model.ShippingAddress.City,
                    StateProvince = model.ShippingAddress.State,
                    AddressLine1 = model.ShippingAddress.AddressLine1,
                    AddressLine2 = model.ShippingAddress.AddressLine2,
                    Phone = model.ShippingAddress.Phone,
                    Zip = model.ShippingAddress.Zip,
                    PostalCode = model.ShippingAddress.PostalCode
                },
                BillingAddress = new Grpc.Ordering.Address
                {
                    Name = model.BillingAddress.Name,
                    CountryCode = model.BillingAddress.Country,
                    City = model.BillingAddress.City,
                    StateProvince = model.BillingAddress.State,
                    AddressLine1 = model.BillingAddress.AddressLine1,
                    AddressLine2 = model.BillingAddress.AddressLine2,
                    Phone = model.BillingAddress.Phone,
                    Zip = model.BillingAddress.Zip,
                    PostalCode = model.BillingAddress.PostalCode
                }
            };


            model.Items.ForEach(item =>
            {
                request.Items.Add(new OrderItemReqeust
                {
                    ProductId = item.ProductId,
                    Sku = item.Sku,
                    Name = item.Name,
                    Thumbnail = item.Thumbnail,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });
            });

            return request;
        }

        private async Task<List<Order>> PrepareOrderListResponse(IEnumerable<OrderResponse> orderResponses)
        {
            var ids = orderResponses.Select(x => x.Id).ToList();

            var userIds = orderResponses.Select(x => x.UserId).ToList();

            var countryCodes = Enumerable.Union(orderResponses.Select(x => x.BillingAddress.CountryCode), orderResponses.Select(x => x.ShippingAddress.CountryCode)).ToList();

            var shipmentsTask = _shipmentService.ListByOrderIds(ids);

            var paymentsTask = _paymentService.ListByOrderIdsAsync(ids);

            var profilesTask = _profilingService.ListByIdsAsync(userIds);

            var countriesTask = _countryService.ListByCodesAsync(countryCodes);

            await Task.WhenAll(shipmentsTask, paymentsTask, profilesTask, countriesTask);

            var shipments = shipmentsTask.Result.ToDictionary(x => x.OrderId);

            var payments = paymentsTask.Result.ToDictionary(x => x.OrderId);

            var users = profilesTask.Result.ToDictionary(x => x.UserId);

            var countries = countriesTask.Result;

            List<Order> orders = new List<Order>();

            foreach (var item in orderResponses)
            {
                var shipment = shipments.GetOrDefault(item.Id);
                var payment = payments.GetOrDefault(item.Id);
                var user = users[item.UserId];

                var order = PrepareOrder(item, user, countries, payment, shipment);

                orders.Add(order);
            }

            return orders;
        }


        private async Task<Order> PrepareSingleOrder(OrderResponse response)
        {
            var countryCodes = Enumerable.Union(new List<string> { response.BillingAddress.CountryCode }, new List<string> { response.ShippingAddress.CountryCode }).ToList();

            Shipment? shipment = null;
            Payment? payment = null;
            UserProfile user;
            List<Country> countries;

            var shipmentTask = _shipmentService.GetByOrderIdAsync(response.Id);

            var paymentTask = _paymentService.GetByOrderIdAsync(response.Id);

            var userProfileTask = _profilingService.GetUserAsync(response.UserId);

            var countriesTask = _countryService.ListByCodesAsync(countryCodes);

            try
            {
                await Task.WhenAll(shipmentTask, paymentTask, userProfileTask, countriesTask);

            }catch(AggregateException exception) when(exception.InnerExceptions is IEnumerable<RpcException> rpcExceptions) 
            {
                if(rpcExceptions.All(x=> x.StatusCode != StatusCode.NotFound))
                {
                    throw;
                }
            }
            finally
            {
                shipment = shipmentTask.Result;
                payment = paymentTask.Result;
                user = userProfileTask.Result;
                countries = countriesTask.Result;
            }

            return PrepareOrder(response, user, countries, payment, shipment);
        }

        private Order PrepareOrder(OrderResponse response, UserProfile user, List<Country> countries, Payment? payment = null, Shipment? shipment = null)
        {
            var billingCountry = countries.Single(x => x.TwoLetterIsoCode == response.BillingAddress.CountryCode ||
            x.ThreeLetterIsoCode == response.BillingAddress.CountryCode);

            var shippingCountry = countries.Single(x => x.TwoLetterIsoCode == response.ShippingAddress.CountryCode ||
                x.ThreeLetterIsoCode == response.ShippingAddress.CountryCode);


            var billingStateProvince = billingCountry.StateProvinces.Single(x => x.Abbreviation == response.BillingAddress.StateProvince);

            var shippingStateProvince = shippingCountry.StateProvinces.Single(x => x.Abbreviation == response.ShippingAddress.StateProvince);

            var order = new Order
            {
                Id = response.Id,
                OrderNumber = response.OrderNumber,
                User = user,
                ShippingAddress = new Data.Common.Address
                {
                    Name = response.ShippingAddress.Name,
                    Country = new AddressCountry
                    {
                        Name = shippingCountry.Name,
                        TwoIsoCode = shippingCountry.TwoLetterIsoCode,
                        ThreeIsoCode = shippingCountry.ThreeLetterIsoCode,
                        NumericIsoCode = shippingCountry.NumericIsoCode
                    },
                    State = new AddressStateProvince
                    {
                        Name = shippingStateProvince.Name,
                        Abbreviation = shippingStateProvince.Abbreviation
                    },
                    City = response.ShippingAddress.City,
                    AddressLine1 = response.ShippingAddress.AddressLine1,
                    AddressLine2 = response.ShippingAddress.AddressLine2,
                    Phone = response.ShippingAddress.Phone,
                    PostalCode = response.ShippingAddress.PostalCode,
                    Zip = response.ShippingAddress.Zip

                },

                BillingAddress = new Data.Common.Address
                {
                    Name = response.BillingAddress.Name,
                    Country = new AddressCountry
                    {
                        Name = billingCountry.Name,
                        TwoIsoCode = billingCountry.TwoLetterIsoCode,
                        ThreeIsoCode = billingCountry.ThreeLetterIsoCode,
                        NumericIsoCode = billingCountry.NumericIsoCode
                    },
                    State = new AddressStateProvince
                    {
                        Name = billingStateProvince.Name,
                        Abbreviation = billingStateProvince.Abbreviation
                    },
                    City = response.BillingAddress.City,
                    AddressLine1 = response.BillingAddress.AddressLine1,
                    AddressLine2 = response.BillingAddress.AddressLine2,
                    Phone = response.BillingAddress.Phone,
                    PostalCode = response.BillingAddress.PostalCode,
                    Zip = response.BillingAddress.Zip

                },
                ShippingCost = response.ShippingCost,
                SubTotal = response.SubTotal,
                TaxCost = response.TaxCost,
                TotalPrice = response.TotalPrice,
                CurrentState = response.CurrentState,
                Shipment = shipment,
                Payment = payment,
                Items = response.Items.Select(x => new OrderItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    Sku = x.Sku,
                    ExternalProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Thumbnail = x.Thumbnail,
                    UnitPrice = x.UnitPrice
                }).ToList(),
                SubmissionDate = response.SubmissionDate.ToDateTime(),
                ShippedDate = response.ShippedDate?.ToDateTime(),


            };

            return order;
        }
   
    }
}
