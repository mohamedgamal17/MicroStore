using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Inventory.Application.Abstractions.Commands
{
    public class ReciveProductCommand : ICommand
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

    }

    internal class ReciveProductCommandValidator : AbstractValidator<ReciveProductCommand>
    {
        public ReciveProductCommandValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Product recived quantity cannot be zero or negative");
        }
    }
}
