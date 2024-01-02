using FluentValidation;

namespace MicroStore.Catalog.Application.Abstractions.SpecificationAttributes
{
    public class SpecificationAttributeOptionModel
    {
        public string Name { get; set; }
    }

    public class SpecificationAttributeOptionModelValidator : AbstractValidator<SpecificationAttributeOptionModel>
    {
        public SpecificationAttributeOptionModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name cannot be null or empty")
                .MaximumLength(265)
                .WithMessage("Name maximum lenght is 256");
        }
    }
}