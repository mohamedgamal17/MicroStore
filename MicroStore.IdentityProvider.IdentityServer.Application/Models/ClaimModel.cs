#pragma warning disable CS8618
using FluentValidation;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Models
{
    public class ClaimModel
    {
        public string  Type { get; set; }
        public string Value { get; set; }
    }

    internal class ClaimModelValidator : AbstractValidator<ClaimModel>
    {
        public ClaimModelValidator()
        {
            RuleFor(x => x.Type)
                .MaximumLength(256)
                .NotNull();

            RuleFor(x => x.Value)
                .MaximumLength(256)
                .NotNull();
        }
    }
 

}
