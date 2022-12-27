using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Abstractions.Categories.Commands
{
    public class CreateCategoryCommand : CategoryCommandBase,ICommandV1
    {
    }

    public class CreateCategoryCommandValidation : AbstractValidator<CreateCategoryCommand>
    {
        private readonly IRepository<Category> _categoryRepository;

        public CreateCategoryCommandValidation(IRepository<Category> categoryRepository)
        {

            _categoryRepository = categoryRepository;

            RuleFor(x => x.Name)
                .MustAsync(CheckCategoryName)
                .WithMessage("Category name must be unique");

        }

        private async Task<bool> CheckCategoryName(string name, CancellationToken cancellationToken)
        {
            return await _categoryRepository
                .AllAsync(x => x.Name != name, cancellationToken);
        }
    }
}
