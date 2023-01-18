using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using Volo.Abp.Application.Dtos;

namespace MicroStore.Shipping.Application.Abstraction.Queries
{
    public class GetShipmentSystemListQuery : IQuery<ListResultDto<ShipmentSystemDto>>
    {
    }
}
