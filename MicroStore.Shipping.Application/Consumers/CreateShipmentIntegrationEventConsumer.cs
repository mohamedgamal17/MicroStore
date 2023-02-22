using MassTransit;

using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Application.Shipments;
using MicroStore.Shipping.IntegrationEvents;
namespace MicroStore.Shipping.Application.Consumers
{
    public class CreateShipmentIntegrationEventConsumer : IConsumer<CreateShipmentIntegrationEvent>
    {

        private readonly IShipmentCommandService _shipmentCommandService;

        public CreateShipmentIntegrationEventConsumer(IShipmentCommandService shipmentCommandService)
        {
            _shipmentCommandService = shipmentCommandService;
        }

        public async Task Consume(ConsumeContext<CreateShipmentIntegrationEvent> context)
        {
            var model = PrepareCreateShipmentModel(context.Message);

            await _shipmentCommandService.CreateAsync(model);
        }


        private ShipmentModel PrepareCreateShipmentModel(CreateShipmentIntegrationEvent integrationEvent)
        {
            return new ShipmentModel
            {
                OrderId = integrationEvent.OrderId,
                UserId = integrationEvent.UserId,
                Address = new Abstraction.Models.AddressModel
                {
                    Name = integrationEvent.Address.Name,
                    Phone = integrationEvent.Address.Phone,
                    CountryCode = integrationEvent.Address.CountryCode,
                    City = integrationEvent.Address.City,
                    State = integrationEvent.Address.State,
                    PostalCode = integrationEvent.Address.PostalCode,
                    Zip = integrationEvent.Address.Zip,
                    AddressLine1 = integrationEvent.Address.AddressLine1,
                    AddressLine2 = integrationEvent.Address.AddressLine2,
                },

                Items = integrationEvent.Items.Select(x => new Abstraction.Models.ShipmentItemModel
                {
                    Name = x.Name,
                    Sku = x.Sku,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Thumbnail = x.Thumbnail,
                    UnitPrice = x.UnitPrice,
                    Dimension = new Abstraction.Models.DimensionModel
                    {
                        Height = x.Dimension.Height,
                        Lenght = x.Dimension.Lenght,
                        Width = x.Dimension.Width,
                        Unit = x.Dimension.Unit,
                    },
                    Weight = new Abstraction.Models.WeightModel
                    {
                        Value = x.Weight.Value,
                        Unit = x.Weight.Unit
                    }
                }).ToList()
            };
        }
    }
}
