using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.Catalog.Application.Abstractions.Categories.Commands;
using MicroStore.Catalog.Application.Abstractions.Categories.Dtos;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Categories.Commands
{
    internal class UpdateCategoryCommandHandler : CommandHandler<UpdateCategoryCommand, CategoryDto>
    {

        private readonly IRepository<Category> _categoryRepository;

        public UpdateCategoryCommandHandler(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public override async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            Category? category = await _categoryRepository.SingleOrDefaultAsync(x => x.Id == request.CategoryId);

            if (category == null)
            {
                throw new EntityNotFoundException(typeof(Category), request.CategoryId);
            }

            category.Name = request.Name;

            category.Description = request.Description;

            await _categoryRepository.UpdateAsync(category, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Category, CategoryDto>(category);
        }



    }
}
