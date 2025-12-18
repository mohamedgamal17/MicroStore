using MicroStore.Bff.Shopping.Data.Shipping;
using MicroStore.Bff.Shopping.Grpc.Shipping;

namespace MicroStore.Bff.Shopping.Services.Shipping
{
    public class ShippingSystemService
    {
        private readonly Grpc.Shipping.ShipmentSystemService.ShipmentSystemServiceClient _shipmentSystemClient;

        public ShippingSystemService(ShipmentSystemService.ShipmentSystemServiceClient shipmentSystemClient)
        {
            _shipmentSystemClient = shipmentSystemClient;
        }


        public async Task<List<ShippingSystem>> ListAsync(CancellationToken cancellationToken = default)
        {
            var result = await _shipmentSystemClient.GetListAsync(new ShipmentSystemListRequest());

            return result.Items.Select(x => new ShippingSystem
            {
                Name = x.Name,
                DisplayName = x.DisplayName,
                Image = x.Image,
                IsEnabled = x.IsEnabled
            }).ToList();
        }
    }
}
