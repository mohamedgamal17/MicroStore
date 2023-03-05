using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Geographic;
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
            var shipment = await _shipmentService.GetAsync(shipmentId, cancellationToken);

            var country = await _countryService.GetByCodeAsync(shipment.Address.CountryCode, false, cancellationToken);

            var stateProvince = await _stateProvinceService.GetByCodeAsync(shipment.Address.CountryCode, shipment.Address.State, cancellationToken);

            var shipmentAggregate = PrepareShipmentAggregate(shipment);

            shipmentAggregate.Address = PrepareAddressAggregate(shipment.Address, country, stateProvince);

            return shipmentAggregate;

        }


        private ShipmentAggregate PrepareShipmentAggregate(Shipment shipment )
        {
            return new ShipmentAggregate
            {
                Id = shipment.Id,
                OrderId = shipment.OrderId,
                UserId = shipment.UserId,
                ShipmentExternalId = shipment.ShipmentExternalId,
                ShipmentLabelExternalId = shipment.ShipmentLabelExternalId,
                SystemName = shipment.SystemName,
                Status = shipment.Status,
                TrackingNumber = shipment.TrackingNumber,
                Items = shipment.Items
            };
        }

        private AddressAggregate PrepareAddressAggregate( Address address , Country country , StateProvince stateProvince ) 
        {
            return new AddressAggregate
            {
                Name = address.Name,
                Phone = address.Phone,
                Country = country,
                State = stateProvince,
                City = address.City,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2
            };
        }
    }
}
