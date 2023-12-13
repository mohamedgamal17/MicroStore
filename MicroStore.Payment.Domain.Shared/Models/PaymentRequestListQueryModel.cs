using FluentValidation;
using MicroStore.BuildingBlocks.Paging.Params;
namespace MicroStore.Payment.Domain.Shared.Models
{
    public class PaymentRequestListQueryModel : PagingAndSortingQueryParams
    {
        public string? OrderNumber { get; set; }
        public string? Status { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }


    public class PaymentRequestListQueryModelValidator: AbstractValidator<PaymentRequestListQueryModel>
    {
        private readonly List<string> _paymentStatus = new List<string>
        {
            "Waiting" ,

            "Payed",

            "UnPayed",

            "Refunded",

            "Failds"
        };


        public PaymentRequestListQueryModelValidator()
        {
            RuleFor(x => x.Status)
                .Must((x) => _paymentStatus.Any(c => c.ToUpper() == x.ToUpper()))
                .WithMessage("Invalid payment status")
                .When(x => x.Status != null);
        }
    }
}
