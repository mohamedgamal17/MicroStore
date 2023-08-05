using FluentValidation;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Models
{
    public class SecretModel
    {
        public string Value { get; set; }
        public string? Description { get; set; }

    }
    internal class SecretModelValidator : AbstractValidator<SecretModel>
    {
        public SecretModelValidator()
        {
            RuleFor(x => x.Value)
                .MaximumLength(400)
                .NotNull();

            RuleFor(x => x.Description)
                .MaximumLength(256)
                .NotNull();
        }
    }
}
