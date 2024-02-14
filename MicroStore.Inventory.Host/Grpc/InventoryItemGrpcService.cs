using FluentValidation;
using FluentValidation.Results;
using Grpc.Core;
using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.Inventory.Application.Dtos;
using MicroStore.Inventory.Application.Models;
using MicroStore.Inventory.Application.Products;
using Volo.Abp.DependencyInjection;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.BuildingBlocks.Utils.Paging.Params;
namespace MicroStore.Inventory.Host.Grpc
{
    public class InventoryItemGrpcService : InventoryItemService.InventoryItemServiceBase
    {
        private readonly IProductCommandService _productCommandService;

        private readonly IProductQueryService _productQueryService;

        public InventoryItemGrpcService(IProductCommandService productCommandService, IProductQueryService productQueryService, IAbpLazyServiceProvider lazyServiceProvider)
        {
            _productCommandService = productCommandService;
            _productQueryService = productQueryService;
            LazyServiceProvider = lazyServiceProvider;
        }

        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }


        public override async Task<InventoryItemResponse> Update(UpdateInventoryItemRequest request, ServerCallContext context)
        {
            var model = new InventoryItemModel
            {
                Stock = request.Stock
            };

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _productCommandService.UpdateProductAsync(request.ProductId, model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareInventoryItemResponse(result.Value);
        }

        public override async Task<InventoryItemListResponse> GetList(InventoryItemListRequest request, ServerCallContext context)
        {
            var queryParams = new PagingQueryParams
            {
                Skip = request.Skip,
                Length = request.Length
            };


            var result = await _productQueryService.ListAsync(queryParams);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareInventoryItemListResponse(result.Value);
        }

        public override async Task<InventoryItemResponse> GetById(GetInventoryItemByIdReqeust request, ServerCallContext context)
        {
            var result = await _productQueryService.GetAsync(request.ProductId);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareInventoryItemResponse(result.Value);
        }
        private InventoryItemListResponse PrepareInventoryItemListResponse(PagedResult<ProductDto> paged)
        {
            var response = new InventoryItemListResponse
            {
                Skip = paged.Skip,
                Length = paged.Lenght,
                TotalCount = paged.TotalCount
            };

            foreach (var item in paged.Items)
            {
                response.Items.Add(PrepareInventoryItemResponse(item));
            }

            return response;
        }
        private InventoryItemResponse PrepareInventoryItemResponse(ProductDto product)
        {
            return new InventoryItemResponse
            {
                Id = product.Id,
                Name = product.Name,
                Sku = product.Sku,
                Stock = product.Stock,
                AllocatedStock = product.AllocatedStock,
                Thumbnail = product.Thumbnail
            };
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
