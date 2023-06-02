using FluentValidation;
using PhoneNumbers;
namespace MicroStore.Ordering.Application.Models
{
    public  class OrderModelValidatorBase<T> : AbstractValidator<T> where T : OrderModel
    {
        public OrderModelValidatorBase()
        {
            RuleFor(x => x.ShippingAddress)
                .NotNull()
                .WithMessage("Shipping Address is required")
                .SetValidator(new AddressValidator());

            RuleFor(x => x.BillingAddress)
                .NotNull()
                .WithMessage("Billing Address is required")
                .SetValidator(new AddressValidator());


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


            RuleFor(x => x.TotalPrice)
                .GreaterThan(0)
                .WithMessage("Total Price must not be zero or negative value")
                .Must((cmd, _) => cmd.TotalPrice == (cmd.ShippingCost + cmd.TaxCost + cmd.TaxCost + cmd.SubTotal))
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

    public class OrderModelValidator : OrderModelValidatorBase<OrderModel> { }

    public class CreateOrderModelValidator : OrderModelValidatorBase<CreateOrderModel>
    {
        public CreateOrderModelValidator()
        {
            RuleFor(x => x.UserId)
                .NotNull()
                .WithMessage("User Id is required")
                .MaximumLength(256)
                .WithMessage("User Id must not exceed 256 characters.");
        }
    }
    internal class FullfillOrderModelValidator : AbstractValidator<FullfillOrderModel>
    {
        public FullfillOrderModelValidator()
        {
            RuleFor(x => x.ShipmentId)
                .NotEmpty()
                .WithMessage("Shipment id is required")
                .MaximumLength(265)
                .WithMessage("Shipment id maximum characters length is 265");
        }
    }

    internal class CancelOrderCommandValidator : AbstractValidator<CancelOrderModel>
    {
        public CancelOrderCommandValidator()
        {
            RuleFor(x => x.Reason)
                .NotEmpty()
                .WithMessage("Order Cancellation Reason is required")
                .MaximumLength(500)
                .WithMessage("Order Cancellation Reason maximum characters is 500");           
        }
    }

    internal class AddressValidator : AbstractValidator<AddressModel>
    {
        public AddressValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("Address Name is required")
                .MaximumLength(400)
                .WithMessage("Address name maximum length is 400");

            RuleFor(x => x.Phone)
                 .NotNull()
                 .WithMessage("Phone is required.")
                 .MinimumLength(10)
                 .WithMessage("Phone must not be less than 10 characters.")
                 .MaximumLength(50)
                 .WithMessage("Phone must not exceed 50 characters.")
                 .Must(ValidatePhone)
                 .WithMessage("Invalid phone number");

            RuleFor(x => x.CountryCode)
                .NotNull()
                .WithMessage("Country Code is required")
                .MinimumLength(2)
                .WithMessage("Country Code minimum lenght is 2")
                .MaximumLength(3)
                .WithMessage("Country Code maximum length is 3");

            RuleFor(x => x.State)
                .NotNull()
                .WithMessage("State is required")
                .Length(2)
                .WithMessage("State code exact lenght is 2");


            RuleFor(x => x.City)
                .NotNull()
                .WithMessage("City is required")
                .Matches(@"[a-zA-Z\u00C0-\u01FF- ]+")
                .WithMessage("Please enter only letters and spaces, no numbers or special characters.")
                .Length(2, 64)
                .WithMessage("State names should be between 2 and 64 characters long.");


            RuleFor(x => x.PostalCode)
             .NotEmpty()
             .WithMessage("Postal Code is required")
             .Length(3, 10)
             .WithMessage("Postal code must be between 3 and 10 digits long, and valid for the country of residence.");

            RuleFor(x => x.Zip)
             .NotEmpty()
             .WithMessage("Please enter a valid zip code.")
             .Length(5, 10)
             .WithMessage("Zip Code should be between 5 and 10 characters long.");

            RuleFor(x => x.AddressLine1)
              .NotEmpty()
              .WithMessage("Address Line 1 is required")
              .Length(6, 128)
              .WithMessage("Addresses should be between 6 and 128 characters long.");


            RuleFor(x => x.AddressLine1)
              .Length(6, 128)
              .WithMessage("Addresses should be between 6 and 128 characters long.");

        }


        private bool ValidatePhone(AddressModel model , string phone)
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();

            try
            {
                var phoneNumberParsed = phoneNumberUtil.Parse(phone, model.CountryCode);

                return phoneNumberUtil.IsPossibleNumberForType(phoneNumberParsed, PhoneNumberType.MOBILE);

            }catch 
            { 
                return false; 
            }

        }
    }


    internal class OrderItemValidator : AbstractValidator<OrderItemModel>
    {
        public OrderItemValidator()
        {
            RuleFor(x => x.ExternalProductId)
                .NotNull()
                .WithMessage("External product id is required")
                .MaximumLength(265)
                .WithMessage("External product id maximum characters is 265");

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

            RuleFor(x => x.Thumbnail)
                .MaximumLength(265)
                .WithMessage("Item Thumbnail maximum characters is 265");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Item quantity should be greater than zero");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0)
                .WithMessage("Item unit price should be greater than zero");
        }
    }
}
