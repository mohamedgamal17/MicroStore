using FluentValidation;
using FluentValidation.Results;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.Catalog.Application.Abstractions.ProductTags;
using Volo.Abp.DependencyInjection;

namespace MicroStore.Catalog.Api.Grpc
{
    public class TagGrpcService : TagService.TagServiceBase
    {
        private readonly IProductTagApplicationService _productTagApplicationService;
        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }

        public TagGrpcService(IProductTagApplicationService productTagApplicationService, IAbpLazyServiceProvider lazyServiceProvider)
        {
            _productTagApplicationService = productTagApplicationService;
            LazyServiceProvider = lazyServiceProvider;
        }



        public override async Task<TagResponse> Create(CreateTagRequest request, ServerCallContext context)
        {
            var model = PreapreTagModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _productTagApplicationService.CreateAsync(model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareTagResponse(result.Value);
        }

        public override async Task<TagResponse> Update(UpdateTagRequest request, ServerCallContext context)
        {
            var model = PreapreTagModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _productTagApplicationService.UpdateAsync(request.Id,model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareTagResponse(result.Value);
        }

        public override async Task<TagListResponse> GetList(TagListRequest request, ServerCallContext context)
        {
            var result = await _productTagApplicationService.ListAsync();

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareTagListResponse(result.Value);
        }

        public override async Task<TagResponse> GetbyId(GetTagByIdRequest request, ServerCallContext context)
        {
            var result = await _productTagApplicationService.GetAsync(request.Id);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareTagResponse(result.Value);
        }

        private ProductTagModel PreapreTagModel(CreateTagRequest request)
        {
            return new ProductTagModel
            {
                Name = request.Name,
                Description = request.Description
            };
        }



        private ProductTagModel PreapreTagModel(UpdateTagRequest request)
        {
            return new ProductTagModel
            {
                Name = request.Name,
                Description = request.Description
            };
        }

        private TagResponse PrepareTagResponse(ProductTagDto tag)
        {
            return new TagResponse
            {
                Id = tag.Id,
                Name = tag.Name,
                Description = tag.Description,
                CreatedAt = Timestamp.FromDateTime(tag.CreationTime.ToUniversalTime()),
                ModifiedAt = tag.LastModificationTime?.ToUniversalTime().ToTimestamp()
            };
        }

        private TagListResponse PrepareTagListResponse(List<ProductTagDto> results)
        {
            var response = new TagListResponse();

            results.ForEach(x => response.Data.Add(PrepareTagResponse(x)));

            return response;
        }

        private async Task<ValidationResult> ValidateModel<TModel>(TModel model)
        {
            var validator = ResolveValidator<TModel>();

            if (validator == null) return new ValidationResult();

            return await validator.ValidateAsync(model);
        }

        private IValidator<T>? ResolveValidator<T>()
        {
            return LazyServiceProvider.LazyGetService<IValidator<T>>();
        }
    }
}
