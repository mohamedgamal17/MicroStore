using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Shipping.Application.Abstraction.Models;
namespace MicroStore.Shipping.Application.Abstraction.Commands
{
    public class CreateShipmentCommand : ICommand
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public AddressModel Address { get; set; }
        public List<ShipmentItemModel> Items { get; set; }
    }


    public class CreateShipmentCommandValidator : AbstractValidator<CreateShipmentCommand>
    {
        public CreateShipmentCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty()
                .WithMessage("Order id is required")
                .MaximumLength(265)
                .WithMessage("Order id maximum characters is 265");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User id is required")
                .MaximumLength(265)
                .WithMessage("User id maximum characters is 265");

            RuleFor(x => x.Address)
                .NotNull()
                .WithMessage("Shipment address is required")
                .SetValidator(new AddressValidator());

            RuleFor(x => x.Items)
                .NotNull()
                .WithMessage("Shipment items is required")
                .Must((prop) => prop.Count > 0)
                .WithMessage("Shipment items should contain at least one item");


            RuleForEach(x => x.Items)
                .SetValidator(new ShipmentItemModelValidator());

        }
    }


  
}
