#nullable disable
using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Ordering.Events.Models;
namespace MicroStore.Ordering.Application.Abstractions.Commands
{
    public class SubmitOrderCommand : ICommand
    {
        public AddressModel ShippingAddress { get; set; }
        public AddressModel BillingAddress { get; set; }
        public string UserId { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalPrice { get; set; }
        public DateTime SubmissionDate { get; set; }
        public List<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();

    }


    internal abstract class SubmitOrderCommandValidator : AbstractValidator<SubmitOrderCommand>
    {
        public SubmitOrderCommandValidator()
        {
            RuleFor(x => x.ShippingAddress)
                .NotNull()
                .WithMessage("Shipping Address is required")
                .SetValidator(new AddressValidator());

            RuleFor(x => x.BillingAddress)
                .NotNull()
                .WithMessage("Billing Address is required")
                .SetValidator(new AddressValidator());

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
                .Must((command, _) => command.SubTotal == command.OrderItems.Sum(x => x.Quantity * x.UnitPrice))
                .WithMessage("Invalid Sub Total value");


            RuleFor(x => x.TotalPrice)
                .GreaterThan(0)
                .WithMessage("Total Price must not be zero or negative value")
                .Must((cmd, _) => cmd.TotalPrice == (cmd.ShippingCost + cmd.TaxCost + cmd.TaxCost + cmd.SubTotal))
                .WithMessage("Invalid Total price value");


            RuleFor(x => x.OrderItems)
                .NotNull()
                .WithMessage("Order line items is required")
                .Must((lineItems) => lineItems.Count > 0)
                .WithMessage("Order line items must contain at least one item");


            RuleForEach(x => x.OrderItems)
                .SetValidator(new OrderItemValidator());


            RuleFor(x => x.SubmissionDate)
                .NotNull()
                .WithMessage("Order submission date is required");
               
                
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
                 .Matches(@"(?:(?:(\s*\(?([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*)|([2-9]1[02-9]|[2‌​-9][02-8]1|[2-9][02-8][02-9]))\)?\s*(?:[.-]\s*)?)([2-9]1[02-9]|[2-9][02-9]1|[2-9]‌​[02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})")
                 .WithMessage("Phone is not valid");

            RuleFor(x => x.CountryCode)
                .NotNull()
                .WithMessage("Country Code is required")
                .MaximumLength(3)
                .WithMessage("Country Code maximum length is 3");
            
            RuleFor(x=> x.State)
                .NotNull()
                .WithMessage("State is required")
                .Matches(@"[a-zA-Z\u00C0-\u01FF- ]+")
                .WithMessage("Please enter only letters and spaces, no numbers or special characters.")        
                .Length(2, 64)
                .WithMessage("State names should be between 2 and 64 characters long.");


            RuleFor(x=> x.City)
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
                .NotNull()
                .WithMessage("Item Thumbnaill is required")
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
