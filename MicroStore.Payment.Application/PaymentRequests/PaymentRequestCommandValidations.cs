
using FluentValidation;
using MicroStore.Payment.Application.PaymentSystems;

namespace MicroStore.Payment.Application.PaymentRequests
{
    internal class CreatePaymentRequestCommandValidator : AbstractValidator<CreatePaymentRequestCommand>
    {
        public CreatePaymentRequestCommandValidator()
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

            RuleFor(x => x.UserId)
                .NotNull()
                .WithMessage("User Id is required")
                .MaximumLength(256)
                .WithMessage("User Id must not exceed 256 characters.");

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
                .SetValidator(new OrderItemValidator());

        }
    }

    internal class ProcessPaymentRequestCommandValidator : AbstractValidator<ProcessPaymentRequestCommand>
    {
        public ProcessPaymentRequestCommandValidator()
        {
            RuleFor(x => x.PaymentGatewayName)
                .NotEmpty()
                .WithMessage("Payment gateway name is required")
                .MaximumLength(265)
                .WithMessage("Payment gateway maximum characters is 265");

            RuleFor(x => x.ReturnUrl)
                .NotEmpty()
                .WithMessage("Return url is required")
                .Matches(@"(([\w]+:)?//)?(([\d\w]|%[a-fA-f\d]{2,2})+(:([\d\w]|%[a-fA-f\d]{2,2})+)?@)?([\d\w][-\d\w]{0,253}[\d\w]\.)+[\w]{2,4}(:[\d]+)?(/([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)*(\?(&?([-+_~.\d\w]|%[a-fA-f\d]{2,2})=?)*)?(#([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)?")
                .WithMessage("Return url should be valid url");


            RuleFor(x => x.CancelUrl)
                .NotEmpty()
                .WithMessage("Cancel url is required")
                .Matches(@"(([\w]+:)?//)?(([\d\w]|%[a-fA-f\d]{2,2})+(:([\d\w]|%[a-fA-f\d]{2,2})+)?@)?([\d\w][-\d\w]{0,253}[\d\w]\.)+[\w]{2,4}(:[\d]+)?(/([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)*(\?(&?([-+_~.\d\w]|%[a-fA-f\d]{2,2})=?)*)?(#([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)?")
                .WithMessage("Cancel url should be valid");
        }
    }

    internal class CompletePaymentRequestCommandValidator : AbstractValidator<CompletePaymentRequestCommand>
    {
        public CompletePaymentRequestCommandValidator()
        {
            RuleFor(x => x.PaymentGatewayName)
               .NotEmpty()
               .WithMessage("Payment gateway name is required")
               .MaximumLength(265)
               .WithMessage("Payment gateway name maximum charaters is 265");

            RuleFor(x => x.Token)
                .NotEmpty()
                .WithMessage("Payment Token is required")
                .MaximumLength(300)
                .WithMessage("Payment Token maximum characters is 300");
        }
    }
    internal class UpdatePaymentSystemCommandValidator : AbstractValidator<UpdatePaymentSystemCommand>
    {
        public UpdatePaymentSystemCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Payment system name is required")
                .MaximumLength(265)
                .WithMessage("Payment system name maxmimum characters is 265");
        }
    }

    internal class OrderItemValidator : AbstractValidator<OrderItemModel>
    {
        public OrderItemValidator()
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
