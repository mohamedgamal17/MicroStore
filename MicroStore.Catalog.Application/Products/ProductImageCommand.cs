using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Catalog.Application.Common;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models;

namespace MicroStore.Catalog.Application.Products
{
    public abstract class ProductImageCommand : ICommand<ProductDto>
    {
        public Guid ProductId { get; set; }
        public ImageModel ImageModel { get; set; }
        public int DisplayOrder { get; set; }

    }

    public class CreateProductImageCommand : ProductImageCommand
    {
    
    }

    public class UpdateProductImageCommand : ProductImageCommand
    {
        public Guid ProductImageId { get; set; }
    }
    public class RemoveProductImageCommand : ICommand<ProductDto>
    {
        public Guid ProductId { get; set; }
        public Guid ProductImageId { get; set; }

    }

    internal class ProductImageCommandValidator<TCommand> : AbstractValidator<TCommand>
       where TCommand : ProductImageCommand
    {
        public ProductImageCommandValidator(IImageService imageService)
        {
            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Display image order cannot be negative number");

            RuleFor(x => x.ImageModel)
                .NotNull()
                .WithName("Image model cannot be null or empty")
                .ChildRules(model =>
                {
                    model.RuleFor(x => x.FileName)
                        .MaximumLength(500)
                        .WithMessage("Image name maximum lenght is 500");
                })
                .MustAsync(imageService.IsValidLenght)
                .WithMessage("Invalid message lenght");
        }
    }
}
