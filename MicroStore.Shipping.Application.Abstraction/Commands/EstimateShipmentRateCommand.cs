using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Shipping.Application.Abstraction.Models;
namespace MicroStore.Shipping.Application.Abstraction.Commands
{
    public class EstimateShipmentRateCommand : ICommand
    {
        public AddressModel Address { get; set; }
        public List<ShipmentItemEstimationModel> Items { get; set; }

    }


    public class EstimateShipmentRateCommandValidator: AbstractValidator<EstimateShipmentRateCommand>
    {
        public EstimateShipmentRateCommandValidator()
        {
            RuleFor(x => x.Address)
                .NotNull()
                .WithMessage("Address is required")
                .SetValidator(new AddressValidator());

            RuleFor(x => x.Items)
                .NotNull()
                .WithMessage("Shipment items is required")
                .Must((prop) => prop.Count > 0)
                .WithMessage("Shipment items should contain one item at least");
        }
    }
}
