using FluentValidation;

namespace MicroStore.Ordering.Application.Models
{
    public class ForecastModel
    {
        public float ConfidenceLevel { get; set; }
        public int Horizon { get; set; }

    }

    public class ForecastModelValidation : AbstractValidator<ForecastModel>
    {
        public ForecastModelValidation()
        {
            RuleFor(x => x.ConfidenceLevel).GreaterThan(0).LessThan(1);

            RuleFor(x => x.Horizon).GreaterThanOrEqualTo(1).LessThanOrEqualTo(6);

        }
    }
}
