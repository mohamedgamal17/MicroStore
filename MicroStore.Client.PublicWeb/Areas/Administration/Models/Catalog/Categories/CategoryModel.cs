using FluentValidation;
using System.ComponentModel.DataAnnotations;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Categories
{
    public class CategoryModel
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public class CatagoryModelValidator: AbstractValidator<CategoryModel>
    {
        public CatagoryModelValidator()
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
