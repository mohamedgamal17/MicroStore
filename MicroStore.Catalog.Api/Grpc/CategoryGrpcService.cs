using FluentValidation;
using FluentValidation.Results;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MicroStore.Catalog.Application.Abstractions.Categories;
using Volo.Abp.DependencyInjection;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
namespace MicroStore.Catalog.Api.Grpc
{
    public class CategoryGrpcService : CategoryService.CategoryServiceBase
    {
        private readonly ICategoryCommandService _catalogCommandService;

        private readonly ICategoryQueryService _catalogQueryService;
        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }
        public CategoryGrpcService(ICategoryCommandService catalogCommandService, ICategoryQueryService catalogQueryService)
        {
            _catalogCommandService = catalogCommandService;
            _catalogQueryService = catalogQueryService;           
        }
        public override async Task<CategoryResponse> Create(CreateCategoryRequest request, ServerCallContext context)
        {
            var model = PrepareCategoryModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _catalogCommandService.CreateAsync(model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PreapreCategoryResponse(result.Value);
        }

        public override async Task<CategoryResponse> Update(UpdateCategoryRequest request, ServerCallContext context)
        {
            var model = PrepareCategoryModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }
            var result = await _catalogCommandService.UpdateAsync(request.Id, model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PreapreCategoryResponse(result.Value);
        }

        public override async Task<ListCategoryResponse> GetList(CategoryListRequest request, ServerCallContext context)
        {
            var model = new CategoryListQueryModel
            {
                Name = request.Name,
                SortBy = request.SortBy,
                Desc = request.Desc
            };

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _catalogQueryService.ListAsync(model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareListCategoryResponse(result.Value);
        }

        public override async Task<CategoryResponse> GetById(GetCategoryByIdRequest request, ServerCallContext context)
        {
            var result = await _catalogQueryService.GetAsync(request.Id);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PreapreCategoryResponse(result.Value);
        }

        private CategoryModel PrepareCategoryModel(CreateCategoryRequest request)
        {
            return new CategoryModel
            {
                Name = request.Name,
                Description = request.Description
            };
        }

        private CategoryModel PrepareCategoryModel(UpdateCategoryRequest request)
        {
            return new CategoryModel
            {
                Name = request.Name,
                Description = request.Description
            };
        }

        private async Task<ValidationResult> ValidateModel<TModel>(TModel model)
        {
            var validator = ResolveValidator<TModel>();

            if (validator == null) return new ValidationResult();

            return await validator.ValidateAsync(model);
        }

        private ListCategoryResponse PrepareListCategoryResponse(List<CategoryDto> values)
        {
            var categoriesResponse = values.Select(PreapreCategoryResponse).ToArray();

            var response = new ListCategoryResponse();

            response.Data.AddRange(categoriesResponse);

            return response;
        }
        private CategoryResponse PreapreCategoryResponse(CategoryDto category)
        {
            var response = new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description ?? string.Empty,
                CreatedAt = Timestamp.FromDateTime(category.CreationTime.ToUniversalTime()),
                ModifiedAt = category.LastModificationTime?.ToUniversalTime().ToTimestamp()
            };

            return response;
        }

        private IValidator<T>? ResolveValidator<T>()
        {
            return LazyServiceProvider.LazyGetService<IValidator<T>>();
        }
    }
}
