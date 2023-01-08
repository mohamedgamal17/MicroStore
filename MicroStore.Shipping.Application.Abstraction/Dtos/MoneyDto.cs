using FluentValidation;

namespace MicroStore.Shipping.Application.Abstraction.Dtos
{
    public class MoneyDto
    {
        public double Value { get; set; }
        public string Currency { get; set; }
    }


    public class MoneyDtoValidator : AbstractValidator<MoneyDto>
    {
        public MoneyDtoValidator()
        {
            RuleFor(x => x.Value)
                .GreaterThan(0)
                .WithMessage("Money value should be greater than zero");

            RuleFor(x => x.Currency)
                .NotEmpty()
                .WithMessage("Currency is required");
        }
    }
}
