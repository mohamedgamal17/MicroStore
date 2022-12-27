using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
namespace MicroStore.Shipping.Application.Abstraction.Commands
{
    public class RetriveShipmentRateCommand : IQuery
    {
        public string SystemName { get; set; }
        public string ExternalShipmentId { get; set; }
    }
}
