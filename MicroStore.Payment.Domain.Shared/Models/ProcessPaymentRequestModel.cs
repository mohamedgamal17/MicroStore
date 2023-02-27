#nullable disable
using FluentValidation;
using MicroStore;

namespace MicroStore.Payment.Domain.Shared.Models
{
    public class ProcessPaymentRequestModel
    {
        public string GatewayName { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }

    }

    public class ProcessPaymentModelValidation : AbstractValidator<ProcessPaymentRequestModel>
    {
        public ProcessPaymentModelValidation()
        {
            RuleFor(x => x.GatewayName)
                .NotEmpty()
                .WithMessage("Payment gateway name is required")
                .MaximumLength(265)
                .WithMessage("Payment gateway maximum characters is 265");


            RuleFor(x => x.ReturnUrl)
                .NotEmpty()
                .WithMessage("Return url is required")
                .Must(LinkMustBeAUri)
                .WithMessage("Return url should be valid url");


            RuleFor(x => x.CancelUrl)
                .NotEmpty()
                .WithMessage("Cancel url is required")
                .Must(LinkMustBeAUri)
                .WithMessage("Cancel url should be valid");
        }

        private static bool LinkMustBeAUri(string link)
        {
            if (string.IsNullOrWhiteSpace(link))
            {
                return false;
            }

            Uri outUri;

            return Uri.TryCreate(link, UriKind.Absolute, out outUri)
                   && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
