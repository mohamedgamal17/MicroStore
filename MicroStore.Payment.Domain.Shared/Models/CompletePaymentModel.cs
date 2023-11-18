#nullable disable
using FluentValidation;

namespace MicroStore.Payment.Domain.Shared.Models
{
    public class CompletePaymentModel
    {
        public string GatewayName { get; set; }
        public string SessionId { get; set; }

    }


    public class CompletePaymentModelValidation : AbstractValidator<CompletePaymentModel>
    {
        public CompletePaymentModelValidation()
        {
            RuleFor(x => x.GatewayName)
                .NotEmpty()
                .WithMessage("Payment gateway name is required")
                .MaximumLength(265)
                .WithMessage("Payment gateway name maximum charaters is 265");

            RuleFor(x => x.SessionId)
                .NotEmpty()
                .WithMessage("Payment Token is required")
                .MaximumLength(300)
                .WithMessage("Payment Token maximum characters is 300");
        }
    }
}
