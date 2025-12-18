using FluentValidation;
using MicroStore.Shipping.Application.Abstraction.Dtos;

namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class ShipmentItemEstimationModel
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public MoneyDto UnitPrice { get; set; }
        public int Quantity { get; set; }
        public WeightModel Weight { get; set; }
    }


    internal class ShipmentItemEstimationModelValidator : AbstractValidator<ShipmentItemEstimationModel>
    {
        public ShipmentItemEstimationModelValidator()
        {

            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("Item name is required")
                .MaximumLength(265)
                .WithMessage("Item name maximum characters is 265");

            RuleFor(x => x.Sku)
                .NotNull()
                .WithMessage("Item sku is required")
                .MaximumLength(265)
                .WithMessage("Item sku maximum characters is 265");

            RuleFor(x => x.UnitPrice)
                .NotNull()
                .WithMessage("Item unit price is required")
                .SetValidator(new MoneyDtoValidator());

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero");

            RuleFor(x => x.Weight)
                .NotNull()
                .WithMessage("Weight is required")
                .SetValidator(new WeightModelValidator());
        }
    }



}
