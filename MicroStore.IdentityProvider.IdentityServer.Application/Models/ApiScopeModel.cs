using FluentValidation;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Models
{
    public class ApiScopeModel
    {
        public string Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public bool Emphasize { get; set; }
        public List<string>? UserClaims { get; set; }
        public List<PropertyModel>? Properties { get; set; }
    }

    internal class ApiScopeModelValidator : AbstractValidator<ApiScopeModel>
    {
        public ApiScopeModelValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(256)
                .NotNull();

            RuleFor(x => x.DisplayName)
                .MaximumLength(256)
                .When(x => x.DisplayName != null);


            RuleForEach(x => x.Properties)
                .SetValidator(new PropertyModelValidator())
                .When(x => x.Properties != null);


            RuleForEach(x => x.UserClaims)
                .MaximumLength(256)
                .When(x => x.UserClaims != null);

        }
    }

   
}
