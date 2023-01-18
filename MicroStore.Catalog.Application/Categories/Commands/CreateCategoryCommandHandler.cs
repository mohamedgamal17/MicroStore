using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Abstractions.Categories.Commands;
using MicroStore.Catalog.Application.Abstractions.Categories.Dtos;
using MicroStore.Catalog.Domain.Entities;
using System.Net;
using Volo.Abp.Domain.Repositories;
namespace MicroStore.Catalog.Application.Categories.Commands
{
    public class CreateCategoryCommandHandler : CommandHandler<CreateCategoryCommand,CategoryDto>
    {
        private readonly IRepository<Category> _categoryRepository;

        public CreateCategoryCommandHandler(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public override async Task<ResponseResult<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            Category category = new Category(request.Name);

            category.Description = request.Description;

            await _categoryRepository.InsertAsync(category, cancellationToken: cancellationToken);

            return  Success(HttpStatusCode.Created  ,ObjectMapper.Map<Category, CategoryDto>(category));
        }
    }
}
