using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Profiling;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
using MicroStore.ShoppingGateway.ClinetSdk.Interfaces;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Billing;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Geographic;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Orders
{
    public class OrderAggregateService : IListableWithPaging<OrderAggregate, OrderListRequestOptions>
    {

        private readonly OrderService _orderService;

        private readonly PaymentRequestService _paymentRequestService;

        private readonly ShipmentService _shipmentService;

        private readonly CountryService _countryService;

        private readonly StateProvinceService _stateProvinceService;

        private readonly ProfileService _profileService;

        public OrderAggregateService(OrderService orderService, PaymentRequestService paymentRequestService, ShipmentService shipmentService, CountryService countryService, StateProvinceService stateProvinceService, ProfileService profileService)
        {
            _orderService = orderService;
            _paymentRequestService = paymentRequestService;
            _shipmentService = shipmentService;
            _countryService = countryService;
            _stateProvinceService = stateProvinceService;
            _profileService = profileService;
        }

        public async Task<PagedList<OrderAggregate>> ListAsync(OrderListRequestOptions options = null, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            var response = await _orderService.ListAsync(options, requestHeaderOptions, cancellationToken);

            var tasks = response.Items.Select(x => PreapreOrderAggregate(x, cancellationToken));

            var aggregates =  await Task.WhenAll(tasks);

            var pagedModel = new PagedList<OrderAggregate>
            {
                Items = aggregates.ToList(),
                Skip = response.Skip,
                Lenght = response.Lenght,
                TotalCount = response.TotalCount
            };

            return pagedModel;
        }

        public async Task<OrderAggregate> GetAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            Order order = await _orderService.GetAsync(orderId, cancellationToken: cancellationToken);

            return await PreapreOrderAggregate(order, cancellationToken);

        }

        private async Task<OrderAggregate> PreapreOrderAggregate(Order order, CancellationToken cancellationToken = default)
        {
            var userProfile = _profileService.GetAsync(order.UserId, cancellationToken: cancellationToken);
            var billingAddress = PrepareAddressAggregate(order.BillingAddress, cancellationToken);
            var shippingAddress = PrepareAddressAggregate(order.ShippingAddress, cancellationToken);

            Task<PaymentRequest> paymentRequest = order.PaymentId != null ? _paymentRequestService.GetAsync(order.PaymentId, cancellationToken: cancellationToken) : Task.FromResult(new PaymentRequest());

            Task<Shipment> shipment = order.ShipmentId != null ? _shipmentService.GetAsync(order.ShipmentId, cancellationToken: cancellationToken) : Task.FromResult(new Shipment());


            await Task.WhenAll(userProfile, billingAddress, shippingAddress, paymentRequest, shipment);

            var aggregate = new OrderAggregate
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                UserId = order.UserId,
                User = userProfile.Result,
                TaxCost = order.TaxCost,
                ShippingCost = order.ShippingCost,
                SubTotal = order.SubTotal,
                TotalPrice = order.TotalPrice,
                BillingAddress = billingAddress.Result,
                ShippingAddress = shippingAddress.Result,
                PaymentId = order.PaymentId,
                Payment = order.PaymentId != null ? PreaprePaymentPaymentRequestAggregate(paymentRequest.Result, userProfile.Result) : null,
                ShipmentId = order.ShipmentId,
                Shipment = order.ShipmentId != null ? PreapreShipmentAggregate(shipment.Result, shippingAddress.Result, userProfile.Result) : null,
                Items = order.Items,
                CurrentState = order.CurrentState,
                SubmissionDate = order.SubmissionDate,
                ShippedDate = order.ShippedDate,
                CancellationDate = order.CancellationDate,
                CancellationReason = order.CancellationReason

            };

            return aggregate;
        }

        private PaymentRequestAggregate PreaprePaymentPaymentRequestAggregate(PaymentRequest paymentRequest, User user)
        {
            return new PaymentRequestAggregate
            {
                Id = paymentRequest.Id,
                OrderId = paymentRequest.OrderId,
                OrderNumber = paymentRequest.OrderNumber,
                UserId = paymentRequest.UserId,
                User = user,
                TransctionId = paymentRequest.TransctionId,
                TaxCost = paymentRequest.TaxCost,
                ShippingCost = paymentRequest.ShippingCost,
                SubTotal = paymentRequest.SubTotal,
                TotalCost = paymentRequest.TotalCost,
                Status = paymentRequest.Status,
                PaymentGateway = paymentRequest.PaymentGateway,
                Description = paymentRequest.Description,
                CapturedAt = paymentRequest.CapturedAt,
                OpenedAt = paymentRequest.OpenedAt,
                Items = paymentRequest.Items
            };
        }
        private ShipmentAggregate PreapreShipmentAggregate(Shipment shipment, AddressAggregate addressAggregate, User user)
        {
            return new ShipmentAggregate
            {
                Id = shipment.Id,
                ShipmentExternalId = shipment.ShipmentExternalId,
                ShipmentLabelExternalId = shipment.ShipmentLabelExternalId,
                TrackingNumber = shipment.TrackingNumber,
                OrderId = shipment.OrderId,
                OrderNumber = shipment.OrderNumber,
                UserId = shipment.UserId,
                SystemName = shipment.SystemName,
                Address = addressAggregate,
                Items = shipment.Items,
                Status = shipment.Status
            };
        }

        private async Task<AddressAggregate> PrepareAddressAggregate(Address address , CancellationToken cancellationToken = default) 
        {
            var country = await _countryService.GetByCodeAsync(address.CountryCode , cancellationToken:cancellationToken);
            var stateProvince = await _stateProvinceService.GetByCodeAsync(address.CountryCode, address.State, cancellationToken: cancellationToken);

            return new AddressAggregate
            {
                Name = address.Name,
                Phone = address.Phone,
                Country = new AddressCountry
                {
                    Id = country.Id,
                    Name = country.Name,
                    NumericIsoCode = country.NumericIsoCode,                
                    TwoLetterIsoCode = country.TwoLetterIsoCode,
                    ThreeLetterIsoCode = country.ThreeLetterIsoCode
                },
                State = new AddressStateProvince
                {
                    Id = stateProvince.Id,
                    Name = stateProvince.Name,
                    Abbreviation = stateProvince.Abbreviation,
                    CountryId = stateProvince.CountryId
                },
                City = address.City,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2
            };
        }

    }
}
