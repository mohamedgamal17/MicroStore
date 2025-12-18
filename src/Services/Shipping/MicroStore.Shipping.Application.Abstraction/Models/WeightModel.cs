using Ardalis.GuardClauses;
using FluentValidation;
using MicroStore.Shipping.Domain.Const;
using MicroStore.Shipping.Domain.ValueObjects;
namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class WeightModel
    {
        public double Value { get; set; }
        public string Unit { get; set; }
        public Weight AsWeight() => Weight.FromUnit(Value, Unit);         

    }


    internal class WeightModelValidator : AbstractValidator<WeightModel>
    {
        public WeightModelValidator()
        {
            RuleFor(x => x.Value)
                .GreaterThan(0)
                .WithMessage("Weight value must not be negative or equal to zero");

            RuleFor(x => x.Unit)
                .NotEmpty()
                .WithMessage("Weigh unit is required")
                .Must((prop) => Enum.TryParse<WeightUnit>(prop,true,out var _))
                .WithMessage("Invalid weight unit");
        }
    }
}
