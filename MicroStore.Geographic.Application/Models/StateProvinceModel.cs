using FluentValidation;

namespace MicroStore.Geographic.Application.Models
{
    public class StateProvinceModel
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        public class StateProvinceModelValidator : AbstractValidator<StateProvinceModel>
        {
            public StateProvinceModelValidator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage("Name cannot be null or empty")
                    .MinimumLength(3)
                    .WithMessage("Name min lenght should be 3")
                    .MaximumLength(256)
                    .WithMessage("Name max lenght should be 256");

                RuleFor(x => x.Abbreviation)
                    .NotEmpty()
                    .WithMessage("Abbreviation cannot be null or empty");

            }
        }
    }
}
