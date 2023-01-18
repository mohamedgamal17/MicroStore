#nullable disable
using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Payment.Application.Abstractions.Dtos;

namespace MicroStore.Payment.Application.Abstractions.Commands
{
    public class ProcessPaymentRequestCommand : ICommand<PaymentProcessResultDto>
    {
        public Guid PaymentId { get; set; }
        public string PaymentGatewayName { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }


   internal class ProcessPaymentRequestCommandValidator : AbstractValidator<ProcessPaymentRequestCommand>
    {
        public ProcessPaymentRequestCommandValidator()
        {
            RuleFor(x => x.PaymentGatewayName)
                .NotEmpty()
                .WithMessage("Payment gateway name is required")
                .MaximumLength(265)
                .WithMessage("Payment gateway maximum characters is 265");

            RuleFor(x => x.ReturnUrl)
                .NotEmpty()
                .WithMessage("Return url is required")
                .Matches(@"(([\w]+:)?//)?(([\d\w]|%[a-fA-f\d]{2,2})+(:([\d\w]|%[a-fA-f\d]{2,2})+)?@)?([\d\w][-\d\w]{0,253}[\d\w]\.)+[\w]{2,4}(:[\d]+)?(/([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)*(\?(&?([-+_~.\d\w]|%[a-fA-f\d]{2,2})=?)*)?(#([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)?")
                .WithMessage("Return url should be valid url");


            RuleFor(x => x.CancelUrl)
                .NotEmpty()
                .WithMessage("Cancel url is required")
                .Matches(@"(([\w]+:)?//)?(([\d\w]|%[a-fA-f\d]{2,2})+(:([\d\w]|%[a-fA-f\d]{2,2})+)?@)?([\d\w][-\d\w]{0,253}[\d\w]\.)+[\w]{2,4}(:[\d]+)?(/([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)*(\?(&?([-+_~.\d\w]|%[a-fA-f\d]{2,2})=?)*)?(#([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)?")
                .WithMessage("Cancel url should be valid");
        }
    }
}
