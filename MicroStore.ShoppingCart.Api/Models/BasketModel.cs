using FluentValidation;

namespace MicroStore.ShoppingCart.Api.Models
{
    public class BasketModel
    {
        public List<BasketItemModel> Items { get; set; }

    }

    public class BasketItemModel
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }


    public class BasketModelValidator : AbstractValidator<BasketModel>
    {
        public BasketModelValidator()
        {
            RuleForEach(x=> x.Items)
                .SetValidator(new BasketItemModelValidator());
        }
    }

    public class BasketItemModelValidator : AbstractValidator<BasketItemModel>
    {
        public BasketItemModelValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Product Id cannot be null or empty")
                .Must((productId) => Guid.TryParse(productId, out _))
                .MaximumLength(256)
                .WithMessage("Product Id maximum lenght is 256");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity cannot be zero or negative number");
        }
    }

    public class RemoveBasketItemsModel
    {
        public string[] ProductIds { get; set; }
    }
}
