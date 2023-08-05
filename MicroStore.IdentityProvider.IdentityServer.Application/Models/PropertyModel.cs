using FluentValidation;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Models
{
    public class PropertyModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    internal class PropertyModelValidator: AbstractValidator<PropertyModel>
    {
        public PropertyModelValidator()
        {
            RuleFor(x => x.Key)
                .MaximumLength(256);


            RuleFor(x => x.Value)
                .MaximumLength(256);

        }
    }

}
