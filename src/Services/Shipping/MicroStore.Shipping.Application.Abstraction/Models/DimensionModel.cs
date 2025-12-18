using FluentValidation;
using MicroStore.Shipping.Domain.Const;
using MicroStore.Shipping.Domain.ValueObjects;
namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class DimensionModel
    {
        public double Width { get; set; }
        public double Length { get; set; }
        public double Height { get; set; }
        public string Unit { get; set; }
        public Dimension AsDimension() => Dimension.FromUnit(Width, Length, Height, Unit);       
        
    }

    public class DimesnsionModelValidator : AbstractValidator<DimensionModel>
    {
        public DimesnsionModelValidator()
        {
            RuleFor(x => x.Width)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Width must not be negative value");

            RuleFor(x => x.Length)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Lenght must not be negative value");

            RuleFor(x => x.Height)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Height must not be negative value");

            RuleFor(x => x.Unit)
                .NotEmpty()
                .WithMessage("Dimension unit is required")
                .Must((prop) => Enum.TryParse<DimensionUnit>(prop,true,out var _))
                .WithMessage("Invalid dimension unit");
        }
    }
}
