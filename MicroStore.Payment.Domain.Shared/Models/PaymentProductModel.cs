#pragma warning disable CS8618
using FluentValidation;

namespace MicroStore.Payment.Domain.Shared.Models
{
    public class PaymentProductModel
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }

    public class PaymentProductModelValidation : AbstractValidator<PaymentProductModel>
    {
        public PaymentProductModelValidation()
        {
            RuleFor(x => x.ProductId)
               .NotNull()
               .WithMessage("Product id is required")
               .MaximumLength(265)
               .WithMessage("Product id maximum characters is 265");

            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("Item name is required")
                .MaximumLength(265)
                .WithMessage("Item name maximum characters is 265");

            RuleFor(x => x.Sku)
                .NotNull()
                .WithMessage("Item sku is required")
                .MaximumLength(265)
                .WithMessage("Item sku maximum characters is 265");

            RuleFor(x => x.Image)
                .NotNull()
                .WithMessage("Item Image is required")
                .MaximumLength(265)
                .WithMessage("Item Image maximum characters is 265");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Item quantity should be greater than zero");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0)
                .WithMessage("Item unit price should be greater than zero");
        }
    }
}
