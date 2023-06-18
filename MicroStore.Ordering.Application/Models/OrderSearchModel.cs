using FluentValidation;
using MicroStore.BuildingBlocks.Paging.Params;

namespace MicroStore.Ordering.Application.Models
{
    public class OrderSearchModel : PagingQueryParams
    {
        public string OrderNumber { get; set; }
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

            RuleFor(x => x.Skip)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Skip cannot be negative number");


            RuleFor(x => x.Length)
                .GreaterThan(0)
                .WithMessage("Lenght cannot be zero or negative");
        }
    }
}
