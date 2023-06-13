using FluentValidation;

namespace MicroStore.Catalog.Application.Models.ProductTags
{
    public class ProductTagModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public class ProductTagModelValidator: AbstractValidator<ProductTagModel>
    {
        public ProductTagModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name cannot be null or empty")
                .MaximumLength(256)
                .WithMessage("Name maximum lenght is 256");


            RuleFor(x => x.Description)
                .MaximumLength(650)
                .WithMessage("Description maximum lenght is 650")
                .When(x => x.Description != null);
        }
    }
}
