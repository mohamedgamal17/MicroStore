using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
namespace MicroStore.Shipping.Application.Abstraction.Commands
{
    public class BuyShipmentLabelCommand : ICommandV1
    {
        public string ExternalShipmentId { get; set; }
        public string SystemName { get; set; }
        public string RateId { get; set; }

    }
}
