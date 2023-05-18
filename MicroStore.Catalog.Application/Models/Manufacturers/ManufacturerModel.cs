using FluentValidation;

namespace MicroStore.Catalog.Application.Models.Manufacturers
{
    public class ManufacturerModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    internal class ManufacturerModelValidation : AbstractValidator<ManufacturerModel>
    {
        public ManufacturerModelValidation()
        {
            RuleFor(x => x.Name)
                .MinimumLength(3)
                .WithMessage("Name minimum lenght is 3")
                .MaximumLength(256)
                .WithMessage("Name maximum lenght is 256");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description maximum lenght is 500")
                .When(x => x != null);
        }
    }
}
