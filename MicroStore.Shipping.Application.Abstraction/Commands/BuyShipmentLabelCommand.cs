using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
namespace MicroStore.Shipping.Application.Abstraction.Commands
{
    public class BuyShipmentLabelCommand : ICommand
    {
        public string ExternalShipmentId { get; set; }
        public string SystemName { get; set; }
        public string RateId { get; set; }

    }
}
