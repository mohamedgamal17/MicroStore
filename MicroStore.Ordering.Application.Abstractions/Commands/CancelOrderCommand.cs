#nullable disable
using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
namespace MicroStore.Ordering.Application.Abstractions.Commands
{
    public class CancelOrderCommand : ICommand
    {
        public Guid OrderId { get; set; }
        public string Reason { get; set; }
        public DateTime CancellationDate { get; set; }
    }

    internal class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderCommandValidator()
        {
            RuleFor(x => x.Reason)
                .NotEmpty()
                .WithMessage("Order Cancellation Reason is required")
                .MaximumLength(500)
                .WithMessage("Order Cancellation Reason maximum characters is 500");

            RuleFor(x => x.CancellationDate)
                .NotEmpty()
                .WithMessage("Order Cancellation date is required");
        }
    }
}
