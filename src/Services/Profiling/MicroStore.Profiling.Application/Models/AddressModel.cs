using FluentValidation;
using PhoneNumbers;

namespace MicroStore.Profiling.Application.Models
{
    public class AddressModel
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string CountryCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Zip { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
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
                .WithMessage("State is required");


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
             .MaximumLength( 10)
             .WithMessage("Postal code maximum length is 10");

            RuleFor(x => x.Zip)
             .NotEmpty()
             .WithMessage("Please enter a valid zip code.")
             .MaximumLength(10)
             .WithMessage("Zip Code maximum length is 10");

            RuleFor(x => x.AddressLine1)
              .NotEmpty()
              .WithMessage("Address Line1 is required")
              .MaximumLength( 500)
              .WithMessage("Address Line1  maximum length is 500");


            RuleFor(x => x.AddressLine2)
              .MaximumLength(500)
              .WithMessage("Address Line2 maximum length is 500");

        }


        private bool ValidatePhone(AddressModel model, string phone)
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();

            try
            {
                var phoneNumberParsed = phoneNumberUtil.Parse(phone, model.CountryCode);

                return phoneNumberUtil.IsPossibleNumberForType(phoneNumberParsed, PhoneNumberType.MOBILE);

            }
            catch
            {
                return false;
            }

        }
    }
}
