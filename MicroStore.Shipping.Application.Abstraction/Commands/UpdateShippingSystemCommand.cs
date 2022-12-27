using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Shipping.Application.Abstraction.Commands
{
    public class UpdateShippingSystemCommand : ICommand
    {
        public string SystemName { get; set; }

        public bool IsEnabled { get; set; }
    }
}
