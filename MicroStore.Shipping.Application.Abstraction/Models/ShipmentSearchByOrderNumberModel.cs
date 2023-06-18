
using FluentValidation;
using MicroStore.BuildingBlocks.Paging.Params;

namespace MicroStore.Shipping.Application.Abstraction.Models
{
    public class ShipmentSearchByOrderNumberModel : PagingQueryParams
    {
        public string OrderNumber { get; set; }

    }


    public class ShipmentSearchByOrderNumberModelValidation : AbstractValidator<ShipmentSearchByOrderNumberModel>
    {
        public ShipmentSearchByOrderNumberModelValidation()
        {
            RuleFor(x => x.OrderNumber)
                .NotEmpty()
                .WithMessage("Order Number is not null or empty")
                .MaximumLength(256)
                .WithMessage("Order Number maximum lenght 256");

            RuleFor(x => x.Skip)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Skip cannot be negative number");


            RuleFor(x => x.Length)
                .GreaterThan(0)
                .WithMessage("Lenght cannot be zero or negative");

        }
    }
}
