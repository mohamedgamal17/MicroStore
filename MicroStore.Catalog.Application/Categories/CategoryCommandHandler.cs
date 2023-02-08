using MassTransit;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Domain.Entities;
using System.Net;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Categories
{
    public class CategoryCommandHandler : RequestHandler,
        ICommandHandler<CreateCategoryCommand, CategoryDto>,
        ICommandHandler<UpdateCategoryCommand, CategoryDto>
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryCommandHandler(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ResponseResult<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            Category category = new Category();

            PrepareCategoryEntity(category, request);

            await _categoryRepository.InsertAsync(category, cancellationToken: cancellationToken);

            return Success(HttpStatusCode.Created, ObjectMapper.Map<Category, CategoryDto>(category));
        }

        public async Task<ResponseResult<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            Category? category = await _categoryRepository.SingleOrDefaultAsync(x => x.Id == request.CategoryId);

            if (category == null)
            {
                return Failure<CategoryDto>(HttpStatusCode.NotFound, new ErrorInfo
                {
                    Message = $"Category entity with id : {request.CategoryId} is not found"
                });
            }
            PrepareCategoryEntity(category, request);

            await _categoryRepository.UpdateAsync(category, cancellationToken: cancellationToken);

            return Success(HttpStatusCode.OK, ObjectMapper.Map<Category, CategoryDto>(category));
        }


        private void PrepareCategoryEntity(Category category, CategoryCommand request)
        {
            category.Name = request.Name;
            category.Description = request.Description;
        }
    }
}
