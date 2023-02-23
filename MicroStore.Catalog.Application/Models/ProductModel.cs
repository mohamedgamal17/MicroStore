﻿#pragma warning disable CS8618
using FluentValidation;
using MicroStore.Catalog.Domain.Const;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Models
{
    public class ProductModel
    {
        public string Name { get; set; } 
        public string Sku { get; set; } 
        public string ShortDescription { get; set; } 
        public string LongDescription { get; set; }
        public double Price { get; set; }
        public double OldPrice { get; set; }
        public string Thumbnail { get; set; }
        public WeightModel Weight { get; set; }
        public DimensionModel Dimensions { get; set; }
        public List<ProductCategoryModel>? Categories { get; set; }
        public List<ProductImageModel>? Images { get; set; }
    }

    public class ProductModelValidator : AbstractValidator<ProductModel>
    {
        protected IRepository<Category> CategoryRepository { get; }
        public ProductModelValidator(IRepository<Category> categoryRepository)
        {
            CategoryRepository = categoryRepository;

            RuleFor(x => x.Name)
              .NotEmpty()
              .WithMessage("Product Name Cannot Be Empty")
              .MinimumLength(3)
              .WithMessage("Product name minmum lenght is 3")
              .MaximumLength(600)
              .WithMessage("Product Name Maximum Length is 600");

            RuleFor(x => x.Sku)
                .NotEmpty()
                .WithMessage("Product sku must be unique")
                .MinimumLength(3)
                .WithMessage("product sku minimum lenght is 3")
                .MaximumLength(265)
                .WithMessage("product sku maximum lenght is 256");

            RuleFor(x => x.ShortDescription)
                .MaximumLength(800)
                .WithMessage("Short Description Maximum Length is 600")
                .MinimumLength(3)
                .WithMessage("short description minimum lenght is 3")
                .Unless(x => x.ShortDescription.IsNullOrEmpty());

            RuleFor(x => x.LongDescription)
                .MaximumLength(2500)
                .WithMessage("Long Description Maximum Length is 2500")
                .Unless(x => x.LongDescription.IsNullOrEmpty());

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Product price should be greater than zero");

            RuleFor(x => x.OldPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Product old price can not be negative");



            RuleFor(x => x.Thumbnail).MaximumLength(600);


            RuleFor(x => x.Weight)
                .ChildRules((weight) =>
                {
                    weight.RuleFor(x => x.Value).GreaterThanOrEqualTo(0)
                        .WithMessage("Product weight cannot be negative value");

                    weight.RuleFor(x => x.Unit)
                        .NotEmpty()
                        .WithMessage("Weight unit cannot be null or empty")
                        .MaximumLength(15)
                        .WithMessage("Weight unit max lenght is 15")
                        .Must(x => StandardWeightUnit.GetStandWeightUnit().Contains(x))
                        .WithMessage("Invalid weight unit");

                })
                .When(x => x.Weight != null);


            RuleFor(x => x.Dimensions)
                .ChildRules((dim) =>
                {
                    dim.RuleFor(x => x.Lenght)
                        .GreaterThanOrEqualTo(0)
                        .WithMessage("Product lenght cannot be negative value");

                    dim.RuleFor(x => x.Width)
                        .GreaterThanOrEqualTo(0)
                        .WithMessage("Product width cannot be negative value");

                    dim.RuleFor(x => x.Height)
                       .GreaterThanOrEqualTo(0)
                       .WithMessage("Product width cannot be negative value");

                    dim.RuleFor(x => x.Unit)
                        .NotEmpty()
                        .MaximumLength(15)
                        .WithMessage("Dimension unit max lenght is 15")
                        .WithMessage("Dimension unit cannot be null or empty")
                        .Must(x => StandardDimensionUnit.GetStandardDimensionUnit().Contains(x))
                        .WithMessage("Invalid dimension unit");
                })
                .When(x => x.Dimensions != null);

            RuleForEach(x => x.Categories)
                .MustAsync(CheckCategoryExist)
                .When(x => x.Categories != null);
        }
        private Task<bool> CheckCategoryExist(ProductCategoryModel model, CancellationToken cancellationToken)
        {
            return CategoryRepository.AnyAsync(x => x.Id == model.CategoryId, cancellationToken);
        }
    }
}