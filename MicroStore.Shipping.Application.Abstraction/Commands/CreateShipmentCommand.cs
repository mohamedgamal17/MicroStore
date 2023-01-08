using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Shipping.Application.Abstraction.Models;
namespace MicroStore.Shipping.Application.Abstraction.Commands
{
    public class CreateShipmentCommand : ICommand
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public AddressModel Address { get; set; }
        public List<ShipmentItemModel> Items { get; set; }
    }


    public class CreateShipmentCommandValidator : AbstractValidator<CreateShipmentCommand>
    {

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

            RuleFor(x => x.State)
                .NotNull()
                .WithMessage("State is required")
                .Matches(@"[a-zA-Z\u00C0-\u01FF- ]+")
                .WithMessage("Please enter only letters and spaces, no numbers or special characters.")
                .Length(2, 64)
                .WithMessage("State names should be between 2 and 64 characters long.");


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
    }
}
