#nullable disable
using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Application.Abstractions.Dtos;

namespace MicroStore.Payment.Application.Abstractions.Commands
{
    public class CompletePaymentRequestCommand : ICommand<PaymentRequestDto>
    {
        public string PaymentGatewayName { get; set; }
        public string Token { get; set; }
    }


    public class CompletePaymentRequestCommandValidator : AbstractValidator<CompletePaymentRequestCommand>
    {
        public CompletePaymentRequestCommandValidator()
        {
            RuleFor(x => x.PaymentGatewayName)
               .NotEmpty()
               .WithMessage("Payment gateway name is required")
               .MaximumLength(265)
               .WithMessage("Payment gateway name maximum charaters is 265");

            RuleFor(x => x.Token)
                .NotEmpty()
                .WithMessage("Payment Token is required")
                .MaximumLength(300)
                .WithMessage("Payment Token maximum characters is 300");
        }
    }
}
