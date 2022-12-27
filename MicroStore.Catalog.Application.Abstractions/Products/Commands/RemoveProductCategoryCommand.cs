using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;

namespace MicroStore.Catalog.Application.Abstractions.Products.Commands
{
    public class RemoveProductCategoryCommand : ICommandV1
    {

        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }

    }


    internal class RemoveProductCategoryCommandValidator : AbstractValidator<RemoveProductCategoryCommand>
    {
        public RemoveProductCategoryCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Product id cannot be null or empty");

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("Category id cannot be null or empty");
        }
    }
}
