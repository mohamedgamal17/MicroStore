using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Shipping.Application.Abstraction.Commands
{
    public class UpdateShippingSystemCommand : ICommand
    {
        public string SystemName { get; set; }

        public bool IsEnabled { get; set; }
    }

    public class UpdateShippingSystemCommandValidator : AbstractValidator<UpdateShippingSystemCommand>
    {
        public UpdateShippingSystemCommandValidator()
        {
            RuleFor(x => x.SystemName)
                .NotEmpty()
                .WithMessage("System name is required")
                .MaximumLength(265)
                .WithMessage("System name maximum characters is 265");
        }
    }
}
