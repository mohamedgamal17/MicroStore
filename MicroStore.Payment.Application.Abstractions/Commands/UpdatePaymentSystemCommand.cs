using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Application.Abstractions.Dtos;

namespace MicroStore.Payment.Application.Abstractions.Commands
{
    public class UpdatePaymentSystemCommand : ICommandV1
    {
        public string Name { get; set; }

        public bool IsEnabled { get; set; }
    }
}
