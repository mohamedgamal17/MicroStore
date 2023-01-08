#nullable disable
using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Ordering.Events.Models;
namespace MicroStore.Ordering.Application.Abstractions.Commands
{
    public class FullfillOrderCommand : ICommand
    {
        public Guid OrderId { get; set; }
        public string ShipmentId { get; set; }
    }


    internal class FullfillOrderCommandValidator : AbstractValidator<FullfillOrderCommand>
    {
        public FullfillOrderCommandValidator()
        {
            RuleFor(x => x.ShipmentId)
                .NotEmpty()
                .WithMessage("Shipment id is required")
                .MaximumLength(265)
                .WithMessage("Shipment id maximum characters length is 265");
        }
    }
}
