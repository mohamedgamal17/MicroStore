using FluentValidation;
using FluentValidation.Results;
using Grpc.Core;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
using MicroStore.ShoppingCart.Api.Models;
using MicroStore.ShoppingCart.Api.Services;
using Volo.Abp.DependencyInjection;

namespace MicroStore.ShoppingCart.Api.Grpc
{
    public class BasketGrpcService : BasketService.BasketServiceBase
    {
        private readonly IBasketService _basketService;

        public BasketGrpcService(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }
        public override async Task<BasketResponse> CreateOrUpdate(BasketRequest request, ServerCallContext context)
        {
            var model = PrepareBasketModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }
            var result = await _basketService.AddOrUpdate(request.UserId, model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareBasketResponse(result.Value);
        }

        public override async Task<BasketResponse> AddOrUpdateItem(AddOrUpdateBasketItemRequest request, ServerCallContext context)
        {
            var model = new BasketItemModel
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _basketService.AddOrUpdateProduct(request.UserId, model);


            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareBasketResponse(result.Value);
        }

        public override async Task<BasketResponse> RemoveProduct(RemoveBasketItemRequest request, ServerCallContext context)
        {
            var model = new RemoveBasketItemModel
            {
                ProductId = request.ProductId,
            };


            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _basketService.RemoveProduct(request.UserId, model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareBasketResponse(result.Value);
        }

        public override async Task<BasketResponse> Migrate(MigrateRequest request, ServerCallContext context)
        {
            var model = new MigrateModel
            {
                FromUserId = request.FromUserId,
                ToUserId = request.ToUserId
            };

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _basketService.MigrateAsync(model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareBasketResponse(result.Value);
        }

        public override async Task<Empty> Clear(CleareUserBasketReqeust request, ServerCallContext context)
        {
            var result = await _basketService.Clear(request.UserId);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return new Empty();
        }

        public override async Task<BasketResponse> GetByUserId(GetByUserIdRequest request, ServerCallContext context)
        {
            var result = await _basketService.GetAsync(request.UserId);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareBasketResponse(result.Value);
        }


        private BasketModel PrepareBasketModel(BasketRequest request)
        {
            var model = new BasketModel
            {
                Items = request.Items.Select(x => new BasketItemModel
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                }).ToList()
            };

            return model;
        }

        private BasketResponse PrepareBasketResponse(BasketDto basket)
        {
            var response = new BasketResponse
            {
                UserId = basket.UserId,

            };

            foreach (var item in basket.Items)
            {
                var responseItem = new BasketItemResponse
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                response.Items.Add(responseItem);
            }

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
