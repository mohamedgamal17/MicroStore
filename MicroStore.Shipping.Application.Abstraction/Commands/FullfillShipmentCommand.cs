using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
namespace MicroStore.Shipping.Application.Abstraction.Commands
{
    public class FullfillShipmentCommand : ICommandV1
    {
        public Guid ShipmentId { get; set; }
        public string SystemName { get; set; }

        public AddressModel AddressFrom { get; set; }
        public PackageModel Pacakge { get; set; }
    }
}
