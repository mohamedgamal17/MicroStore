using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Billing;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Geographic;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Orders
{
    public class OrderAggregateService
    {

        private readonly OrderService _orderService;

        private readonly PaymentRequestService _paymentRequestService;

        private readonly ShipmentAggregateService _shipmentAggregateService;

        private readonly CountryService _countryService;

        private readonly StateProvinceService _stateProvinceService;

        public OrderAggregateService(OrderService orderService, ShipmentAggregateService shipmentAggregateService, CountryService countryService, StateProvinceService stateProvinceService, PaymentRequestService paymentRequestService)
        {
            _orderService = orderService;
            _shipmentAggregateService = shipmentAggregateService;
            _countryService = countryService;
            _stateProvinceService = stateProvinceService;
            _paymentRequestService = paymentRequestService;
        }


        public async Task<OrderAggregate> GetAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            Order order = await _orderService.GetAsync(orderId, cancellationToken);

            PaymentRequest paymentRequest = null;

            ShipmentAggregate shipment = null;

            if(order.PaymentId != null)
            {
                paymentRequest = await _paymentRequestService.GetAsync(order.PaymentId, cancellationToken);
            }

            if(order.ShipmentId != null)
            {
                shipment = await _shipmentAggregateService.GetAsync(order.ShipmentId, cancellationToken);
            }
            

            var orderAggregate=  BuildOrderAggregate(order);

            orderAggregate.Payment = paymentRequest;

            orderAggregate.Shipment = shipment;

            orderAggregate.BillingAddress = await PrepareAddressAggregate(order.BillingAddress, cancellationToken);

            orderAggregate.ShippingAddress = await PrepareAddressAggregate(order.ShippingAddress, cancellationToken);

            return orderAggregate;
        }

        private OrderAggregate BuildOrderAggregate(Order order)
        {
            return new OrderAggregate
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                PaymentId = order.PaymentId,
                ShipmentId = order.ShipmentId,
                UserId = order.UserId,
                TaxCost = order.TaxCost,
                ShippingCost = order.ShippingCost,
                SubTotal = order.SubTotal,
                TotalPrice = order.Total,
                Items = order.Items,
                SubmissionDate = order.SubmissionDate,
                ShippedDate = order.ShippedDate,
                CurrentState = order.CurrentState,
                CancellationDate = order.CancellationDate,
                CancellationReason = order.CancellationReason,
               
            };
        }

        private async Task<AddressAggregate> PrepareAddressAggregate(Address address , CancellationToken cancellationToken = default) 
        {
            var country = await _countryService.GetByCodeAsync(address.CountryCode ,cancellationToken);
            var stateProvince = await _stateProvinceService.GetByCodeAsync(address.CountryCode, address.State, cancellationToken);

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
