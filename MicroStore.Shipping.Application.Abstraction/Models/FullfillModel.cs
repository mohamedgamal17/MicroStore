using FluentValidation;
using MicroStore.Shipping.Domain.ValueObjects;

namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class FullfillModel
    {
        public string SystemName { get; set; }
        public AddressModel AddressFrom { get; set; }
        public AddressModel AddressTo { get; set; }
        public PackageModel Package {get; set;}      
    }

    public class FullfillModelValidator : AbstractValidator<FullfillModel>
    {
        public FullfillModelValidator()
        {
            RuleFor(x => x.SystemName)
                .NotEmpty()
                .WithMessage("System Name cannot be null or empty")
                .MaximumLength(256)
                .WithMessage("System Name maximum lenght is 256");

            RuleFor(x => x.AddressFrom)
                .NotNull()
                .WithMessage("Address From is required")
                .SetValidator(new AddressModelValidator());

            RuleFor(x => x.AddressTo)
                .NotNull()
                .WithMessage("Address To is required")
                .SetValidator(new AddressModelValidator());

            RuleFor(x => x.Package)
                .NotNull()
                .WithMessage("Pacakge is required")
                .SetValidator(new PackageModelValidator());
        }
    }
}
