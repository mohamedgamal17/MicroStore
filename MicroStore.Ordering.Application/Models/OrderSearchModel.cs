using FluentValidation;
using MicroStore.BuildingBlocks.Utils.Paging.Params;
namespace MicroStore.Ordering.Application.Models
{
    public class OrderSearchModel : PagingAndSortingQueryParams
    {
        public string OrderNumber { get; set; }
        public string[]? States { get; set; }
        public DateTime? StartSubmissionDate { get; set; }
        public DateTime? EndSubmissionDate { get; set; }

    }


    public class OrderSearchModelValidaton : AbstractValidator<OrderSearchModel>
    {
        public OrderSearchModelValidaton()
        {
            RuleFor(x => x.OrderNumber)
                .NotEmpty()
                .WithMessage("Order Number cannot be null or empty")
                .MaximumLength(256)
                .WithMessage("Order Number maximum lenght is 256");

            RuleFor(x => x.States)
                .Must(x => x!.Length > 0)
                .WithMessage("States should at least contain one state to filter on it")
                .ForEach(x => x.MaximumLength(256))
                .When(x => x.States != null);

            RuleFor(x => x.Skip)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Skip cannot be negative number");


            RuleFor(x => x.Length)
                .GreaterThan(0)
                .WithMessage("Lenght cannot be zero or negative");
        }
    }
}
