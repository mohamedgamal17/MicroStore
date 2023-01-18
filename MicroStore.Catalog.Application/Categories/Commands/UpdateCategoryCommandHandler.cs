using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Catalog.Application.Abstractions.Categories.Commands;
using MicroStore.Catalog.Application.Abstractions.Categories.Dtos;
using MicroStore.Catalog.Domain.Entities;
using System.Net;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Categories.Commands
{
    internal class UpdateCategoryCommandHandler : CommandHandler<UpdateCategoryCommand,CategoryDto>
    {

        private readonly IRepository<Category> _categoryRepository;

        public UpdateCategoryCommandHandler(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public override async Task<ResponseResult<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            Category? category = await _categoryRepository.SingleOrDefaultAsync(x => x.Id == request.CategoryId);

            if (category == null)
            {
                return Failure(HttpStatusCode.NotFound, new ErrorInfo
                {
                    Message = $"Category entity with id : {request.CategoryId} is not found"
                });
            }

            category.Name = request.Name;

            category.Description = request.Description;

            await _categoryRepository.UpdateAsync(category, cancellationToken: cancellationToken);

            return Success(HttpStatusCode.OK, ObjectMapper.Map<Category, CategoryDto>(category));
        }



    }
}
