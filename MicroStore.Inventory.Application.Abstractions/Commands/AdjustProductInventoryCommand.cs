using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Inventory.Application.Abstractions.Dtos;

namespace MicroStore.Inventory.Application.Abstractions.Commands
{
    public class AdjustProductInventoryCommand : ICommand<ProductAdjustedInventoryDto>
    {
        public Guid ProductId { get; init; }

        public int Stock { get; init; }

        public string Reason { get; init; }
    }



    internal class AdjustProductInventoryCommandValidator : AbstractValidator<AdjustProductInventoryCommand>
    {

        public AdjustProductInventoryCommandValidator()
        {
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Product stock can not be negative number");


            RuleFor(x => x.Reason)
                .MinimumLength(3)
                .WithMessage("Adjust product reason min lenght is 3");
        }
    }
}
