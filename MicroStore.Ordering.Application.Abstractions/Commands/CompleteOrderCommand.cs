using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
namespace MicroStore.Ordering.Application.Abstractions.Commands
{
    public class CompleteOrderCommand : ICommand
    {
        public Guid OrderId { get; set; }
        public DateTime ShipedDate { get; set; }
    }


    internal class CompleterOrderCommandValidator: AbstractValidator<CompleteOrderCommand>
    {
        public CompleterOrderCommandValidator()
        {
            RuleFor(x => x.ShipedDate)
                .NotEmpty()
                .WithMessage("Shiped date is required");
        }
    }
}
