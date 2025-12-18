#pragma warning disable CS8618
using FluentValidation;

namespace MicroStore.Payment.Domain.Shared.Models
{
    public class PaymentRequestModel
    {
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalCost { get; set; }
        public List<PaymentProductModel> Items { get; set; }
    }

    public class CreatePaymentRequestModel : PaymentRequestModel
    {
        public string UserId { get; set; }
    }

    public class PaymentRequestModelValidatorBase<T> : AbstractValidator<T> where T : PaymentRequestModel
    {
        public PaymentRequestModelValidatorBase()
        {
            RuleFor(x => x.OrderId)
               .NotEmpty()
               .WithMessage("Order id is required")
               .MaximumLength(265)
               .WithMessage("Order id maximum characters is 265");

            RuleFor(x => x.OrderNumber)
                .NotEmpty()
                .WithMessage("Order number is required")
                .MaximumLength(265)
                .WithMessage("Order number maximum characters is 265");

            RuleFor(x => x.ShippingCost)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Shipping Cost must not be negative value");

            RuleFor(x => x.TaxCost)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Tax Cost must not be negative value");

            RuleFor(x => x.SubTotal)
                .GreaterThan(0)
                .WithMessage("Sub Total must not be zero or negative value")
                .Must((command, _) => command.SubTotal == command.Items.Sum(x => x.Quantity * x.UnitPrice))
                .WithMessage("Invalid Sub Total value");


            RuleFor(x => x.TotalCost)
                .GreaterThan(0)
                .WithMessage("Total Price must not be zero or negative value")
                .Must((cmd, _) => cmd.TotalCost == (cmd.ShippingCost + cmd.TaxCost + cmd.TaxCost + cmd.SubTotal))
                .WithMessage("Invalid Total price value");


            RuleFor(x => x.Items)
                .NotNull()
                .WithMessage("Order line items is required")
                .Must((lineItems) => lineItems.Count > 0)
                .WithMessage("Order line items must contain at least one item");


            RuleForEach(x => x.Items)
                .SetValidator(new PaymentProductModelValidation());
        }
    }

    public class PaymentRequestModelValidator : PaymentRequestModelValidatorBase<PaymentRequestModel> { }
    public class CreatePaymentRequestModelValidator : PaymentRequestModelValidatorBase<CreatePaymentRequestModel>
    {
        public CreatePaymentRequestModelValidator()
        {

            RuleFor(x => x.UserId)
                .NotNull()
                .WithMessage("User Id is required")
                .MaximumLength(256)
                .WithMessage("User Id must not exceed 256 characters.");
        }
    }
}
