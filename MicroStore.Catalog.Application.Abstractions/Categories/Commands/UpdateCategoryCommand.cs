using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Catalog.Application.Abstractions.Categories.Dtos;
using MicroStore.Catalog.Domain.Entities;

using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Abstractions.Categories.Commands
{
    public class UpdateCategoryCommand : CategoryCommandBase,ICommandV1
    {
        public Guid CategoryId { get; set; }
    }
    internal class UpdateCategoryCommandValidation : CategoryCommandValidatorBase<UpdateCategoryCommand>
    {


        private readonly IRepository<Category> _categoryRepository;

        public UpdateCategoryCommandValidation(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("category id is required");

            RuleFor(x => x.Name)
               .MustAsync(CheckCategoryName)
               .WithMessage("Category name must be unique");


        }


        private async Task<bool> CheckCategoryName(UpdateCategoryCommand command, string name, CancellationToken cancellationToken)
        {
            return await _categoryRepository
                .AllAsync(x => x.Name != name || x.Id == command.CategoryId, cancellationToken);
        }
    }
}
