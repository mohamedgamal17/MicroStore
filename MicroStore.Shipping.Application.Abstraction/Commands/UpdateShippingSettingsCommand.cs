using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Shipping.Application.Abstraction.Models;

namespace MicroStore.Shipping.Application.Abstraction.Commands
{
    public class UpdateShippingSettingsCommand : ICommand
    {
        public string DefaultShippingSystem { get; set; }
        public AddressModel Location { get; set; }

    }

    internal class UpdateShipppingSettingsCommandValidator : AbstractValidator<UpdateShippingSettingsCommand>
    {
        public UpdateShipppingSettingsCommandValidator()
        {
            RuleFor(x => x.DefaultShippingSystem)
                .NotEmpty()
                .WithMessage("Default Shipping System is required")
                .MaximumLength(265)
                .WithMessage("Default Shipping System maximum characters is 265");

            RuleFor(x => x.Location)
                .NotNull()
                .WithMessage("Location is required")
                .SetValidator(new AddressValidator());

        }
    }
}
