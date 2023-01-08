using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
namespace MicroStore.Shipping.Application.Abstraction.Commands
{
    public class BuyShipmentLabelCommand : ICommand
    {
        public string ExternalShipmentId { get; set; }
        public string SystemName { get; set; }
        public string RateId { get; set; }

    }


    internal class BuyShipmentLabelValidator : AbstractValidator<BuyShipmentLabelCommand>
    {
        public BuyShipmentLabelValidator()
        {
            RuleFor(x => x.ExternalShipmentId)
                .NotEmpty()
                .WithMessage("External shipment id is required")
                .MaximumLength(265)
                .WithMessage("External shipment id maximum lenght is 265");

            RuleFor(x => x.SystemName)
                .NotEmpty()
                .WithMessage("System name is required")
                .MaximumLength(265)
                .WithMessage("System name maximum characters is 265");

            RuleFor(x => x.RateId)
                .NotEmpty()
                .WithMessage("Rate id is required")
                .MaximumLength(265)
                .WithMessage("Rate id maximum characters is 265");
        }
    }
}
