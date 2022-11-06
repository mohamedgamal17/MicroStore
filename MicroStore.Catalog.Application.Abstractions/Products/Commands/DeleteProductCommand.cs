using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Catalog.Application.Abstractions.Products.Commands
{
    public class DeleteProductCommand : ICommand
    {
        public Guid Id { get; set; }
    }


    public class DeleteProductCommandValidation : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidation()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("product id is required");
        }

    }
}
