using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Shipping.Application.Abstraction.Models;

namespace MicroStore.Shipping.Application.Abstraction.Commands
{
    public class UpdateShippingSettingsCommand : ICommand
    {
        public string DefaultShippingSystem { get; set; }
        public AddressModel Location { get; set; }

    }
}
