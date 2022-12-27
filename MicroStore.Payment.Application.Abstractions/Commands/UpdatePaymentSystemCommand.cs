using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Application.Abstractions.Dtos;

namespace MicroStore.Payment.Application.Abstractions.Commands
{
    public class UpdatePaymentSystemCommand : ICommand
    {
        public string Name { get; set; }

        public bool IsEnabled { get; set; }
    }
}
