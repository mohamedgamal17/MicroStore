using FluentValidation;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Categories
{
    public class CategoryCommand
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
    }


    public class CreateCategoryCommand : CategoryCommand, ICommand<CategoryDto>
    {
    }
    public class UpdateCategoryCommand : CreateCategoryCommand, ICommand<CategoryDto>
    {
        public Guid CategoryId { get; set; }
    }

    internal abstract class CategoryCommandValidator<TCommand> : AbstractValidator<TCommand>
    where TCommand : CategoryCommand
    {
        public CategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("category name is required")
                .MinimumLength(3)
                .WithMessage("Category name minimum chars is 3")
                .MaximumLength(250)
                .WithMessage("category name maximum length is 250");

            RuleFor(x => x.Description)
                .MinimumLength(3)
                .WithMessage("Category description minimum chars is 3")
                .MaximumLength(850)
                .WithMessage("category description maximum length is 850")
                .Unless(x => x.Description.IsNullOrEmpty());
        }
    }
    internal class CreateCategoryCommandValidation : CategoryCommandValidator<CreateCategoryCommand>
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

    internal class UpdateCategoryCommandValidation : CategoryCommandValidator<UpdateCategoryCommand>
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
