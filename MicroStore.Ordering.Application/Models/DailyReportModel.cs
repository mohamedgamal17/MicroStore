using FluentValidation;

namespace MicroStore.Ordering.Application.Models
{
    public class DailyReportModel
    {
        public int Year { get; set; }

        public int Month { get; set; }
    }

    public class DailySalesReportModelValidation :AbstractValidator<DailyReportModel>  
    {
        public DailySalesReportModelValidation()
        {
            RuleFor(x => x.Year).LessThanOrEqualTo(DateTime.Now.Year).GreaterThan(0);

            RuleFor(x => x.Month).GreaterThanOrEqualTo(1).LessThanOrEqualTo(12);
             
        }
    }
}
