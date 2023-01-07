using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.Catalog.Application.Abstractions.Common.Models;
namespace MicroStore.Catalog.Application.Abstractions.Products.Commands
{
    public abstract class ProductImageCommandBase : ICommand
    {
        public Guid ProductId { get; set; }
        public int DisplayOrder { get; set; }

    }

    internal abstract class ProductImageCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
        where TCommand : ProductImageCommandBase
    {
        public ProductImageCommandValidatorBase()
        {
            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Display image order cannot be negative number");
        }
    }



    public class AssignProductImageCommand : ProductImageCommandBase
    {
        public ImageModel ImageModel { get; set; }

    }


    internal  class AssignProductImageCommandValidator : ProductImageCommandValidatorBase<AssignProductImageCommand>
    {
        public AssignProductImageCommandValidator(IImageService imageService)
        {
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
