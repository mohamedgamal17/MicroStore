using FluentValidation;

namespace MicroStore.Catalog.Application.Abstractions.Categories
{
    public class CategoryModel
    {
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    internal class CategoryModelValidation : AbstractValidator<CategoryModel>
    {
        public CategoryModelValidation()
        {
            RuleFor(x => x.Name)
              .NotEmpty()
              .WithMessage("category name is required")
              .MinimumLength(3)
              .WithMessage("Category name minimum chars is 3")
              .MaximumLength(250)
              .WithMessage("category name maximum length is 250");

            RuleFor(x => x.Description)
                .MinimumLength(3)
                .WithMessage("Category description minimum chars is 3")
                .MaximumLength(850)
                .WithMessage("category description maximum length is 850")
                .Unless(x => x.Description.IsNullOrEmpty());
        }
    }
}

