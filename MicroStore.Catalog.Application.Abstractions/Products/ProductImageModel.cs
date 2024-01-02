#pragma warning disable CS8618

using FluentValidation;

namespace MicroStore.Catalog.Application.Abstractions.Products
{
    public class ProductImageModel
    {
        public string Image { get; set; }
        public int DisplayOrder { get; set; }
    }

    [Obsolete]
    public class UpdateProductImageModel
    {
        public int DisplayOrder { get; set; }
    }

    public class CreateProductImageModelValidator : AbstractValidator<ProductImageModel>
    {
        public CreateProductImageModelValidator()
        {
            RuleFor(x => x.Image)
                .NotEmpty()
                .WithMessage("Image cannot be null or empty");


            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Invalid image display order");
        }
    }


}
