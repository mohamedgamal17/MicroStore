using Grpc.Core;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Shipping.Application.ShippingSystems;

namespace MicroStore.Shipping.Host.Grpc
{
    public class ShipmentSystemGrpcService : ShipmentSystemService.ShipmentSystemServiceBase
    {
        private readonly IShippingSystemQueryService _shippingSystemQueryService;

        public ShipmentSystemGrpcService(IShippingSystemQueryService shippingSystemQueryService)
        {
            _shippingSystemQueryService = shippingSystemQueryService;
        }

        public override async Task<ShipmentSystemListResponse> GetList(ShipmentSystemListRequest request, ServerCallContext context)
        {
            var result = await _shippingSystemQueryService.ListAsync();

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            var items = result.Value.Select(x => new ShipmentSystemResponse
            {
                Name = x.Name,
                DisplayName = x.DisplayName,
                Image = x.Image,
                IsEnabled = x.IsEnabled
            }).ToList();

            var response = new ShipmentSystemListResponse();

            response.Items.AddRange(items);

            return response;
        }

    }
}
