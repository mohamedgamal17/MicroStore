using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Geographic;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping
{
    public class ShipmentAggregateService
    {
        private readonly ShipmentService _shipmentService;

        private readonly CountryService _countryService;

        private readonly StateProvinceService _stateProvinceService;

        public ShipmentAggregateService(ShipmentService shipmentService, CountryService countryService, StateProvinceService stateProvinceService)
        {
            _shipmentService = shipmentService;
            _countryService = countryService;
            _stateProvinceService = stateProvinceService;
        }


        public async Task<ShipmentAggregate> GetAsync(string shipmentId , CancellationToken cancellationToken = default)
        {
            var shipment = await _shipmentService.GetAsync(shipmentId, cancellationToken: cancellationToken);

            var addressAggregate = await PrepareAddressAggregate(shipment.Address);

            var shipmentAggregate = BuildShipmentAggregate(shipment);

            shipmentAggregate.Address = addressAggregate;

            return shipmentAggregate;

        }


        private ShipmentAggregate BuildShipmentAggregate(Shipment shipment )
        {
            return new ShipmentAggregate
            {
                Id = shipment.Id,
                OrderId = shipment.OrderId,
                OrderNumber = shipment.OrderNumber,
                UserId = shipment.UserId,
                ShipmentExternalId = shipment.ShipmentExternalId,
                ShipmentLabelExternalId = shipment.ShipmentLabelExternalId,
                SystemName = shipment.SystemName,
                Status = shipment.Status,
                TrackingNumber = shipment.TrackingNumber,
                Items = shipment.Items
            };
        }

        private async Task<AddressAggregate> PrepareAddressAggregate(Address address, CancellationToken cancellationToken = default)
        {
            var country = await _countryService.GetByCodeAsync(address.CountryCode, cancellationToken);

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
