using FluentValidation;
using MicroStore.BuildingBlocks.Utils.Paging.Params;
namespace MicroStore.Payment.Domain.Shared.Models
{
    public class PaymentRequestSearchModel : PagingQueryParams
    {
        public string OrderNumber { get; set; }
    }


    public class PaymentRequestSearchModelValidation : AbstractValidator<PaymentRequestSearchModel>
    {
        public PaymentRequestSearchModelValidation()
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
