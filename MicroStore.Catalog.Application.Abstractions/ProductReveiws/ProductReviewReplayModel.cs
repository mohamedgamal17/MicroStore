using FluentValidation;

namespace MicroStore.Catalog.Application.Abstractions.ProductReveiws
{
    public class ProductReviewReplayModel
    {
        public string ReplayText { get; set; }
    }

    public class ProductReviewReplayModelValidator : AbstractValidator<ProductReviewReplayModel>
    {
        public ProductReviewReplayModelValidator()
        {
            RuleFor(x => x.ReplayText)
                .NotEmpty()
                .WithMessage("ReplayText cannot be null or empty")
                .MaximumLength(650)
                .WithMessage("ReplyText maximum lenght is 650");
        }
    }
}
