#pragma warning disable CS8618
using FluentValidation;
using System.ComponentModel;

namespace MicroStore.IdentityProvider.Identity.Domain.Shared.Models
{
    public class UserModel
    {
        [DisplayName("Given Name")]
        public string GivenName { get; set; }

        [DisplayName("Family Name")]
        public string FamilyName { get; set; }

        [DisplayName("User Name")]
        public string UserName { get; set; }
        public string Email { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        public string? Password { get; set; }

        [DisplayName("User Roles")]
        public List<string>? UserRoles { get; set; }

    }

    public class UserModeValidator: AbstractValidator<UserModel>
    {
        const string USER_NAME_REGEX_PATTERN = @"^([a-zA-Z]+)[0-9]*\\.*[a-zA-Z0-9]+$|^[a-zA-Z]+[0-9]*$";
        public UserModeValidator()
        {
            RuleFor(x => x.GivenName)
                .MinimumLength(3)
                .MaximumLength(265)
                .NotNull();

            RuleFor(x => x.FamilyName)
               .MinimumLength(3)
               .MaximumLength(265)
               .NotNull();

            RuleFor(x => x.UserName)
              .MinimumLength(3)
              .MaximumLength(265)
              .Matches(USER_NAME_REGEX_PATTERN)
              .NotNull();


            RuleFor(x => x.Email)
                .MinimumLength(3)
                .MaximumLength(256)
                .EmailAddress()
                .NotNull();

            RuleFor(x => x.Password)
                .MinimumLength(8)
                .When(x => x.Password != null);
        }


    }
}
