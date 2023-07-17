using FluentValidation;
using System.ComponentModel;

namespace MicroStore.Client.PublicWeb.Models
{
    public class AddressModel
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Country Code")]
        public string Country { get; set; }

        [DisplayName("State Province")]
        public string StateProvince { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("Address Line 1")]
        public string AddressLine1 { get; set; }

        [DisplayName("Address Line 1")]
        public string? AddressLine2 { get; set; }

        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }

        [DisplayName("Zip Code")]
        public string ZipCode { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class AddressModelValidator : AbstractValidator<AddressModel>
    {
        public AddressModelValidator()
        {
            RuleFor(x => x.FirstName)
               .NotNull()
               .MaximumLength(150);    
            RuleFor(x => x.LastName)
               .NotNull()
               .MaximumLength(150);

            RuleFor(x => x.PhoneNumber)
                 .NotNull()
                 .WithMessage("Phone is required.")
                 .MinimumLength(10)
                 .WithMessage("Phone must not be less than 10 characters.")
                 .MaximumLength(50)
                 .WithMessage("Phone must not exceed 50 characters.")
                 .Must(ValidatePhone)
                 .WithMessage("Invalid phone number");

            RuleFor(x => x.Country)
                .NotNull()
                .Length(3);

            RuleFor(x => x.StateProvince)
                .NotNull()
                .Length(2);


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

            RuleFor(x => x.ZipCode)
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


        private bool ValidatePhone(AddressModel model, string phone)
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();

            try
            {
                var phoneNumberParsed = phoneNumberUtil.Parse(phone, model.Country);

                return phoneNumberUtil.IsPossibleNumberForType(phoneNumberParsed, PhoneNumberType.MOBILE);

            }
            catch
            {
                return false;
            }

        }

    }
