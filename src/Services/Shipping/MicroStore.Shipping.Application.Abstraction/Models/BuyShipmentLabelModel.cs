using FluentValidation;
namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class BuyShipmentLabelModel
    {
        public string ShipmentRateId { get; set; }

    }


    public class BuyShipmentLabelModelValidator : AbstractValidator<BuyShipmentLabelModel>
    {
        public BuyShipmentLabelModelValidator()
        {
            RuleFor(x => x.ShipmentRateId)
                .NotEmpty()
                .WithMessage("Shipment Rate Id is required")
                .MaximumLength(256)
                .WithMessage("Shipment Rate Id maximum lenght 256");

        }
    }
}
