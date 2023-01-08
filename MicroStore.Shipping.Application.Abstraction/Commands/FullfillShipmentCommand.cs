using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Models;
namespace MicroStore.Shipping.Application.Abstraction.Commands
{
    public class FullfillShipmentCommand : ICommand
    {
        public Guid ShipmentId { get; set; }
        public string SystemName { get; set; }
        public AddressModel AddressFrom { get; set; }
        public PackageModel Pacakge { get; set; }
    }


    internal class FullfillShipmentCommandValidator : AbstractValidator<FullfillShipmentCommand>
    {
        public FullfillShipmentCommandValidator()
        {
            RuleFor(x => x.SystemName)
                .NotEmpty()
                .WithMessage("System name is required")
                .MaximumLength(265)
                .WithMessage("System name maximum lenght is 265");

            RuleFor(x => x.AddressFrom)
                .NotNull()
                .WithMessage("Address From is required")
                .SetValidator(new AddressValidator());

            RuleFor(x => x.Pacakge)
                .NotNull()
                .WithMessage("Pacakge is required")
                .SetValidator(new PackageModelValidator());
        }
    }
}
