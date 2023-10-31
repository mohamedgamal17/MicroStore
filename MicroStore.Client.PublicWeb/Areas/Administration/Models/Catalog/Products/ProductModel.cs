using FluentValidation;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Common;
using System.ComponentModel;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Products
{
    public class ProductModel
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }

        [DisplayName("Categories")]
        public string[]? CategoriesIds { get; set; }

        [DisplayName("Manufacturer")]
        public string[]? ManufacturersIds { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public bool IsFeatured { get; set; }
        public double Price { get; set; } 
        public double OldPrice { get; set; }
        public WeightModel Weight { get; set; } = new WeightModel();
        public DimensionModel Dimensions { get; set; } = new DimensionModel();
    }


    public class ProductModelValidator : AbstractValidator<ProductModel>
    {
        public ProductModelValidator()
        {

            RuleFor(x => x.Name)
                .MinimumLength(3)
                .NotEmpty()
                .MaximumLength(600);

            RuleFor(x => x.Sku)
                .MinimumLength(3)
                .NotEmpty()
                .MaximumLength(600);

            RuleFor(x => x.CategoriesIds)
                .ForEach(x => x.MaximumLength(256))
                .When(x => x.CategoriesIds != null && x.CategoriesIds.Length > 0);

            RuleFor(x => x.ManufacturersIds)
               .ForEach(x => x.MaximumLength(256))
               .When(x => x.ManufacturersIds != null && x.ManufacturersIds.Length > 0);

            RuleFor(x => x.ShortDescription)
             .MaximumLength(800)
             .MinimumLength(3)
             .Unless(x => x.ShortDescription.IsNullOrEmpty());

            RuleFor(x => x.Price)
              .GreaterThan(0);

            RuleFor(x => x.OldPrice)
              .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Weight)
              .ChildRules((weight) =>
              {
                  weight.RuleFor(x => x.Value).GreaterThanOrEqualTo(0);
              })
              .When(x => x.Weight != null);


            RuleFor(x => x.Dimensions)
               .ChildRules((dim) =>
               {
                   dim.RuleFor(x => x.Length)
                       .GreaterThanOrEqualTo(0);

                   dim.RuleFor(x => x.Width)
                       .GreaterThanOrEqualTo(0);

                   dim.RuleFor(x => x.Height)
                      .GreaterThanOrEqualTo(0);

               })
               .When(x => x.Dimensions != null);
        }
    }

}
