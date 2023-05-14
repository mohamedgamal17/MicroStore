#pragma warning disable CS8618
using FluentValidation;

namespace MicroStore.Geographic.Application.Models
{
    public class CountryModel
    {
        public string Name { get; set; }
        public string TwoLetterIsoCode { get; set; }
        public string ThreeLetterIsoCode { get; set; }
        public int NumericIsoCode { get; set; }


        public class CountryModelValidator : AbstractValidator<CountryModel>
        {
            public CountryModelValidator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage("Name cannot be null or empty")
                    .MinimumLength(3)
                    .WithMessage("Name min lenght should be 3")
                    .MaximumLength(256)
                    .WithMessage("Name max lenght should be 256");

                RuleFor(x => x.TwoLetterIsoCode)
                    .NotEmpty()
                    .WithMessage("Two Letter Iso Code cannot be null or empty")
                    .Length(2)
                    .WithMessage("Two Letter Iso exact lenght should be 2");

                RuleFor(x => x.ThreeLetterIsoCode)
                    .NotEmpty()
                    .WithMessage("Three Letter Iso Code cannot be null or empty")
                    .Length(3)
                    .WithMessage("Three Letter Iso exact lenght should be 3");

                RuleFor(x => x.NumericIsoCode)
                    .GreaterThan(0)
                    .WithMessage("Numeric Iso Code should be greater than zero");
            }
        }
    }
}
