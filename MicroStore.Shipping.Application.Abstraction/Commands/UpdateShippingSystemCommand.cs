using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Shipping.Application.Abstraction.Commands
{
    public class UpdateShippingSystemCommand : ICommandV1
    {
        public string SystemName { get; set; }

        public bool IsEnabled { get; set; }
    }
}
