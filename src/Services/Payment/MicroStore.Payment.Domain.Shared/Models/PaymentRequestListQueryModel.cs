using FluentValidation;
using MicroStore.BuildingBlocks.Utils.Paging.Params;
namespace MicroStore.Payment.Domain.Shared.Models
{
    public class PaymentRequestListQueryModel : PagingAndSortingQueryParams
    {
        public string OrderNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public double MinPrice { get; set; } = -1;
        public double MaxPrice { get; set; } = -1;
        public DateTime StartDate { get; set; } = DateTime.MinValue;
        public DateTime EndDate { get; set; } = DateTime.MinValue;

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
                .When(x => !x.Status.IsNullOrEmpty());
        }
    }
}
