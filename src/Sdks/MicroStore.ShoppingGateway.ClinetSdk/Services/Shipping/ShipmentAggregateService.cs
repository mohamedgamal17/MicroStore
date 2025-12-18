using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Geographic;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Orders;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling;
using MicroStore.ShoppingGateway.ClinetSdk.Interfaces;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping
{
    public class ShipmentAggregateService : 
         IListableWithPaging<ShipmentAggregate, PagingReqeustOptions>,
        IRetrievable<ShipmentAggregate>
    {
        private readonly ShipmentService _shipmentService;

        private readonly CountryService _countryService;

        private readonly ProfileService _profileService;

        public ShipmentAggregateService(ShipmentService shipmentService, CountryService countryService, ProfileService profileService)
        {
            _shipmentService = shipmentService;
            _countryService = countryService;
            _profileService = profileService;
        }

        public async Task<PagedList<ShipmentAggregate>> ListAsync(PagingReqeustOptions options = null, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            var response = await _shipmentService.ListAsync(options, requestHeaderOptions, cancellationToken);

            var tasks = response.Items.Select(x => PreapreShipmentAggregate(x, cancellationToken));

            var aggregates = await Task.WhenAll(tasks);

            var pagedModel = new PagedList<ShipmentAggregate>
            {
                Items = aggregates.ToList(),
                Skip = response.Skip,
                Lenght = response.Lenght,
                TotalCount = response.TotalCount
            };

            return pagedModel;
        }

        public async Task<ShipmentAggregate> GetAsync(string id, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            var shipment = await _shipmentService.GetAsync(id, cancellationToken: cancellationToken);

            return await PreapreShipmentAggregate(shipment, cancellationToken);
        }
        public async Task<PagedList<ShipmentAggregate>> ListAsync(ShipmentListRequestOptions options = null, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            var response = await _shipmentService.ListAsync(options, requestHeaderOptions, cancellationToken);

            var tasks = response.Items.Select(x => PreapreShipmentAggregate(x, cancellationToken));

            var aggregates = await Task.WhenAll(tasks);
            
            var pagedModel = new PagedList<ShipmentAggregate>
            {
                Items = aggregates.ToList(),
                Skip = response.Skip,
                Lenght = response.Lenght,
                TotalCount = response.TotalCount
            };

            return pagedModel;
        }
        private async  Task<ShipmentAggregate> PreapreShipmentAggregate(Shipment shipment, CancellationToken cancellationToken )
        {
            var userProfile = _profileService.GetAsync(shipment.UserId, cancellationToken: cancellationToken);
            var shippingAddress = PrepareAddressAggregate(shipment.Address, cancellationToken);

            await Task.WhenAll(userProfile, shippingAddress);

            return new ShipmentAggregate
            {
                Id = shipment.Id,
                OrderId = shipment.OrderId,
                OrderNumber = shipment.OrderNumber,
                UserId = shipment.UserId,
                User = userProfile.Result,
                Address = shippingAddress.Result,
                ShipmentExternalId = shipment.ShipmentExternalId,
                ShipmentLabelExternalId = shipment.ShipmentLabelExternalId,        
                SystemName = shipment.SystemName,
                Status = shipment.Status,
                TrackingNumber = shipment.TrackingNumber,
                Items = shipment.Items,
                CreatedAt = shipment.CreatedAt

              
            };
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
            var country = await _countryService.GetByCodeAsync(address.CountryCode, cancellationToken: cancellationToken);

            var stateProvince = country.StateProvinces?.SingleOrDefault(x => x.Abbreviation == address.State);

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
                State = stateProvince != null ? new AddressStateProvince
                {
                    Id = stateProvince.Id,
                    Name = stateProvince.Name,
                    Abbreviation = stateProvince.Abbreviation,
                    CountryId = stateProvince.CountryId
                } : null,
                City = address.City,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2
            };
        }

     
    }
}
