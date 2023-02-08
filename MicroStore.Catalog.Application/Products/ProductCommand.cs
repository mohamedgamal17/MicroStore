using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Catalog.Application.Common;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models;
using MicroStore.Catalog.Domain.Const;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Products
{
    public abstract class ProductCommand
    {
        public string Name { get; set; } = null!;
        public string Sku { get; set; } = null!;
        public string ShortDescription { get; set; } = string.Empty;
        public string LongDescription { get; set; } = string.Empty;
        public double Price { get; set; }
        public double OldPrice { get; set; }
        public ImageModel Thumbnail { get; set; }
        public WeightModel Weight { get; set; }
        public DimensionModel Dimensions { get; set; }
        public List<CategoryModel>? Categories { get; set; }
    }


    public class CreateProductCommand : ProductCommand , ICommand<ProductDto>
    {
    }

    public class UpdateProductCommand : ProductCommand, ICommand<ProductDto>
    {
        public Guid ProductId { get; set; }
    }


    internal abstract class ProductCommandValidator<TCommand> : AbstractValidator<TCommand>
        where TCommand : ProductCommand
    {
        protected IImageService ImageService { get; }
        protected IRepository<Product> ProductRepository { get; }
        protected IRepository<Category> CategoryRepository { get; }
        public ProductCommandValidator(IImageService imageService, IRepository<Product> productRepository, IRepository<Category> categoryRepository)
        {
            ImageService = imageService;
            ProductRepository = productRepository;
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



            RuleFor(x => x.Thumbnail)
                .ChildRules((model) =>
                {
                    model.RuleFor(x => x.FileName)
                        .MaximumLength(500)
                        .WithMessage("Image name max length is 500");
                })
                .MustAsync(imageService.IsValidLenght)
                .When(x => x.Thumbnail != null);


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
                .MustAsync((CheckCategoryExist))
                .When(x => x.Categories != null);

        }

        private Task<bool> CheckCategoryExist(CategoryModel model , CancellationToken cancellationToken)
        {
            return CategoryRepository.AnyAsync(x => x.Id == model.CategoryId, cancellationToken);
        }
    }

    internal class CreateProductCommandValidation : ProductCommandValidator<CreateProductCommand>
    {

        public CreateProductCommandValidation(IImageService imageService, IRepository<Product> productRepository, IRepository<Category> categoryRepository) :base(imageService,productRepository,categoryRepository)

        {


            RuleFor(x => x.Name)
                .MustAsync((x, ct) => CheckProductName(productRepository, x, ct))
                .WithMessage("Product name must be unique");

            RuleFor(x => x.Sku)
                .MustAsync((x, ct) => CheckProductSku(productRepository, x, ct))
                .WithMessage("Product sku must be unique");

        }
        private Task<bool> CheckProductName(IRepository<Product> productRepository, string name, CancellationToken cancellationToken)
        {
            return productRepository
                .AllAsync(x => x.Name != name);
        }

        private Task<bool> CheckProductSku(IRepository<Product> productRepository, string sku, CancellationToken cancellationToken)
        {
            return productRepository
                .AllAsync(x => x.Sku != sku);
        }
    }

    internal class UpdateProductCommandCommandValidation : ProductCommandValidator<UpdateProductCommand>
    {

        private readonly IRepository<Product> _productRepository;

        public UpdateProductCommandCommandValidation(IImageService imageService, IRepository<Product> productRepository, IRepository<Category> categoryRepository) : base(imageService, productRepository, categoryRepository)
        {
            _productRepository = productRepository;

            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Product Id Is Required");

            RuleFor(x => x.Name)
                .MustAsync(CheckProductName)
                .WithMessage("Product name must be unique");


            RuleFor(x => x.Sku)
                .MustAsync(CheckProductSku)
                .WithMessage("Product sku must be unique");

        }




        private async Task<bool> CheckProductName(UpdateProductCommand command, string name, CancellationToken cancellationToken)
        {
            var query = await _productRepository.GetQueryableAsync();

            return await query.Where(x => x.Id != command.ProductId)
                .AllAsync(x => x.Name != name, cancellationToken);
        }

        private async Task<bool> CheckProductSku(UpdateProductCommand command, string sku, CancellationToken cancellationToken)
        {
            var query = await _productRepository.GetQueryableAsync();

            return await query.Where(x => x.Id != command.ProductId)
                .AllAsync(x => x.Sku != sku, cancellationToken);
        }


    }
}

