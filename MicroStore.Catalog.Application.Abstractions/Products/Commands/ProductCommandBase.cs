﻿

using FluentValidation;
using MicroStore.Catalog.Application.Abstractions.Products.Models;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Abstractions.Products.Commands
{
    public abstract class ProductCommandBase
    {
        public string Name { get; set; } = null!;
        public string Sku { get; set; } = null!;
        public string ShortDescription { get; set; } = string.Empty;
        public string LongDescription { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public List<ProductCategoryModel> ProductCategories { get; set; } = new List<ProductCategoryModel>();
    }

    internal abstract class ProductCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
         where TCommand : ProductCommandBase
    {
        protected readonly IRepository<Category> CategoryRepository;
        public ProductCommandValidatorBase(IRepository<Category> categoryRepository)
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

            RuleForEach(x => x.ProductCategories)
                .SetValidator(new ProductCategoryModelValidator(CategoryRepository))
                .When(x => x.ProductCategories.Count() > 0);

        }
    }
}