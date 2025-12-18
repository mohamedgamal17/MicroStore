using FluentValidation;
using MicroStore.Shipping.Domain.ValueObjects;
namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class PackageModel
    {
        public WeightModel Weight { get; set; }
        public DimensionModel Dimension { get; set; }
    }

    internal class PackageModelValidator : AbstractValidator<PackageModel>
    {
        public PackageModelValidator()
        {
            RuleFor(x => x.Weight)
                .NotNull()
                .WithMessage("Weight is required")
                .SetValidator(new WeightModelValidator());

            RuleFor(x => x.Dimension)
                .NotNull()
                .WithMessage("Dimension is required")
                .SetValidator(new DimesnsionModelValidator());
        }
    }
}
