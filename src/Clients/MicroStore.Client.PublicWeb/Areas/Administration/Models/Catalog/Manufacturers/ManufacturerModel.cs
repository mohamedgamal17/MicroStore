using FluentValidation;
using System.ComponentModel.DataAnnotations;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Manufacturers
{
    public class ManufacturerModel
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public class ManufacturerModelValidator : AbstractValidator<ManufacturerModel>
    {
        public ManufacturerModelValidator()
        {

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}
