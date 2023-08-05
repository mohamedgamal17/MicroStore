#pragma warning disable CS8618
using FluentValidation;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Models
{
    public class ApiResourceModel
    {
        public string Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public bool RequireResourceIndicator { get; set; }
        public List<PropertyModel>? Properties { get; set; }
        public List<string>? UserClaims { get; set; }
        public List<string>? Scopes { get; set; }
    }

    internal class ApiResourceModelValidator: AbstractValidator<ApiResourceModel>
    {
        public ApiResourceModelValidator()
        {
            RuleFor(x => x.Name)
             .MaximumLength(256)
             .NotNull();

            RuleFor(x => x.DisplayName)
                .MaximumLength(256)
                .When(x => x.DisplayName != null);

            RuleFor(x => x.Description)
                .MaximumLength(256)
                .When(x => x.Description != null);

            RuleForEach(x => x.Properties)
                .SetValidator(new PropertyModelValidator())
                .When(x => x.Properties != null);

            RuleForEach(x => x.UserClaims)
                .MaximumLength(256)
                .When(x => x.UserClaims != null);

            RuleForEach(x => x.Scopes)
                .MaximumLength(256)
                .When(x => x.Scopes != null);
        }
    }

  
}
