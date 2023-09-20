using FluentValidation;
using MicroStore.Profiling.Application.Domain;
using PhoneNumbers;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Profiling.Application.Models
{

    public class ProfileModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public PhoneModel Phone { get; set; }
        public string? Avatar { get; set; }
        public List<AddressModel>? Addresses { get; set; }
    }

    public class CreateProfileModel : ProfileModel
    {
        public string UserId { get; set; }
    }


    internal class ProfileModelValidationBase<T> : AbstractValidator<T> where T : ProfileModel
    {
        const string URI_REGEX_PATTERN = @"(https:[/][/]|http:[/][/]|www.)[a-zA-Z0-9\\-\\.]+\\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\\-\\._\\?\\,\\'/\\\\\\+&amp;%\\$#\\=~])*$";

        public ProfileModelValidationBase()
        {
            RuleFor(x => x.FirstName)
                .NotNull()
                .WithMessage("First Name is required")
                .MaximumLength(256);

            RuleFor(x => x.LastName)
                .NotNull()
                .WithMessage("Last Name is required")
                .MaximumLength(256);


            RuleFor(x => x.BirthDate)
                .Must((date) => date <= DateTime.Now.AddYears(-12))
                .WithMessage("Invalid birth date");


            RuleFor(x => x.Phone)
                .ChildRules(child =>
                {

                    child.RuleFor(x => x.Number)
                         .NotNull()
                         .WithMessage("Phone Number is required.")
                         .MinimumLength(10)
                         .WithMessage("Phone Number must not be less than 10 characters.")
                         .MaximumLength(50)
                         .WithMessage("Phone Number must not exceed 50 characters.")
                         .Must((model, _) => ValidatePhone(model.Number, model.CountryCode))
                         .WithMessage("Invalid phone number");

                    child.RuleFor(x => x.CountryCode)
                     .NotNull()
                     .WithMessage("Country Code is required")
                     .MinimumLength(2)
                     .WithMessage("Country Code minimum lenght is 2")
                     .MaximumLength(3)
                     .WithMessage("Country Code maximum length is 3");
                });


            RuleForEach(x => x.Addresses)
                .SetValidator(new AddressValidator())
                .When(x => x.Addresses != null);

        }

        private bool ValidatePhone(string number, string countryCode)
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();

            try
            {
                var phoneNumberParsed = phoneNumberUtil.Parse(number, countryCode);

                return phoneNumberUtil.IsPossibleNumberForType(phoneNumberParsed, PhoneNumberType.MOBILE);

            }
            catch
            {
                return false;
            }

        }
    }


    internal class ProfileModelValidation : ProfileModelValidationBase<ProfileModel> { }

    internal class CreateProfileModelValidation : ProfileModelValidationBase<CreateProfileModel>
    {

        private readonly IRepository<Profile> _profileRepository;

        public CreateProfileModelValidation(IRepository<Profile> profileRepository)
        {
            _profileRepository = profileRepository;

            RuleFor(x => x.UserId)
                .NotNull()
                .WithMessage("User Id is required")
                .MaximumLength(256)
                .MustAsync(async (userId, ct) => await _profileRepository.AllAsync(x => x.UserId != userId))
                .WithMessage("User already has profile");
          
        }
    }
}
