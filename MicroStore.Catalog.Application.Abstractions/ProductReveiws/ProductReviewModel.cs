using FluentValidation;

namespace MicroStore.Catalog.Application.Abstractions.ProductReveiws
{
    public class ProductReviewModel
    {
        public string Title { get; set; }

        public string ReviewText { get; set; }

        public int Rating { get; set; }

    }

    public class CreateProductReviewModel : ProductReviewModel
    {
        public string UserId { get; set; }
    }

    public class ProductReviewModelValidator<T> : AbstractValidator<T> where T : ProductReviewModel
    {
        public ProductReviewModelValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title cannot be null or empty")
                .MaximumLength(256)
                .WithMessage("Title maximum lenght is 256");



            RuleFor(x => x.ReviewText)
                .NotEmpty()
                .WithMessage("Review text cannot be null or empty")
                .MaximumLength(650)
                .WithMessage("Review text maximum lenght is 650");

            RuleFor(x => x.Rating)
                .Must(x => x >= 1 && x < 6)
                .WithMessage("Rating must be in range from 1 to 5");
        }
    }

    public class CreateProductReviewModelValidator : ProductReviewModelValidator<CreateProductReviewModel>
    {
        public CreateProductReviewModelValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId cannot be null or empty")
                .MaximumLength(256)
                .WithMessage("UserId maximum lenght is 256");

        }
    }
}
