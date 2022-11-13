using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Inventory.Application.Abstractions.Dtos;

namespace MicroStore.Inventory.Application.Abstractions.Commands
{
    public class ReciveProductCommand : ICommand<ProductRecivedDto>
    {
        public Guid ProductId { get; init; }
        public int RecivedQuantity { get; init; }
    }


    internal class ReciveProductCommandValidator : AbstractValidator<ReciveProductCommand>
    {
        public ReciveProductCommandValidator()
        {
            RuleFor(x => x.RecivedQuantity)
                .GreaterThan(0)
                .WithMessage("Product recived quantity can not be zero or negative");
        }
    }
}
