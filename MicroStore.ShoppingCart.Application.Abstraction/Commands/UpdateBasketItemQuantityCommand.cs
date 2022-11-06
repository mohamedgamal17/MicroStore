using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.ShoppingCart.Application.Abstraction.Dtos;
namespace MicroStore.ShoppingCart.Application.Abstraction.Commands
{
    public class UpdateBasketItemQuantityCommand : ICommand<BasketDto>
    {
        public Guid BasketId { get; set; }

        public Guid BasketItemId { get; set; }

        public int Quantity { get; set; }
    }


    internal class UpdateBasketItemCommandValidator : AbstractValidator<UpdateBasketItemQuantityCommand>
    {

        public UpdateBasketItemCommandValidator()
        {
            RuleFor(x => x.Quantity)
                 .LessThanOrEqualTo(100)
                 .WithMessage("Quantity must be less than or equal to 100");


            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Quantity can not be negative number");
        }

    }
}
