using FluentValidation;

namespace MicroStore.Catalog.Application.Abstractions.SpecificationAttributes
{
    public class SpecificationAttributeModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public HashSet<SpecificationAttributeOptionModel>? Options { get; set; }
    }


    public class SpecificationAttributeModelValidator : AbstractValidator<SpecificationAttributeModel>
    {
        public SpecificationAttributeModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name cannot be null or empty")
                .MaximumLength(265)
                .WithMessage("Name maximum lenght is 256");

            RuleFor(x => x.Description)
                .MaximumLength(650)
                .WithMessage("Description maximum lenght is 650")
                .When(x => x.Description != null);
        }
    }
}
