using FluentValidation;
using FluentValidation.Results;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.Ordering.Application.Dtos;
using MicroStore.Ordering.Application.Models;
using MicroStore.Ordering.Application.Orders;
using Volo.Abp.DependencyInjection;
using MicroStore.BuildingBlocks.AspNetCore.Extensions;
namespace MicroStore.Ordering.Api.Grpc
{
    public class OrderGrpcService : OrderService.OrderServiceBase
    {
        private readonly IOrderCommandService _orderCommandService;
        private readonly IOrderQueryService _orderQueryService;

        public OrderGrpcService(IOrderCommandService orderCommandService, IOrderQueryService orderQueryService)
        {
            _orderCommandService = orderCommandService;
            _orderQueryService = orderQueryService;
        }

        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }


        public override async Task<OrderResponse> Create(CreateOrderRequest request, ServerCallContext context)
        {
            var model = PrepareOrderModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _orderCommandService.CreateOrderAsync(model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareOrderResponse(result.Value);
        }

        public override async Task<EmptyResponse> Fullfill(FullfillOrderRequest request, ServerCallContext context)
        {
            var model = new FullfillOrderModel
            {
                ShipmentId = request.ShipmentId
            };

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _orderCommandService.FullfillOrderAsync(Guid.Parse(request.OrderId), model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }


            return new EmptyResponse();
        }

        public override async Task<EmptyResponse> Complete(CompleteOrderReqeust request, ServerCallContext context)
        {
            var orderId = Guid.Parse(request.OrderId);

            var result = await _orderCommandService.CompleteOrderAsync(orderId);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return new EmptyResponse();
        }


        public override async Task<EmptyResponse> Cancel(CancelOrderReqeust request, ServerCallContext context)
        {

            var model = new CancelOrderModel
            {
                Reason = request.Reason
            };

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _orderCommandService.CancelOrderAsync(Guid.Parse(request.OrderId), model);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }
            return new EmptyResponse();
        }

        public override async Task<OrderListResponse> GetList(OrderListReqeust request, ServerCallContext context)
        {
            var model = PrepareOrderListQueryModel(request);

            var validationResult = await ValidateModel(model);

            if (!validationResult.IsValid)
            {
                validationResult.ThrowRpcException();
            }

            var result = await _orderQueryService.ListAsync(model, request.UserId);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareOrderListResponse(result.Value);
        }

        public override async Task<OrderResponse> GetbyId(GetOrderByIdReqeuest request, ServerCallContext context)
        {
            Guid orderId = Guid.Parse(request.OrderId);

            var result = await _orderQueryService.GetAsync(orderId);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }
            return PrepareOrderResponse(result.Value);
        }

        public override async Task<OrderResponse> GetByNumber(GetOrderByNumberRequest request, ServerCallContext context)
        {

            var result = await _orderQueryService.GetByOrderNumberAsync(request.OrderNumber);

            if (result.IsFailure)
            {
                result.Exception.ThrowRpcException();
            }

            return PrepareOrderResponse(result.Value);
        }

        private CreateOrderModel PrepareOrderModel(CreateOrderRequest request)
        {
            var model = new CreateOrderModel
            {

                UserId = request.UserId,
                ShippingAddress = PrepareAddressModel(request.ShippingAddress),
                BillingAddress = PrepareAddressModel(request.BillingAddress),
                ShippingCost = request.ShippingCost,
                TaxCost = request.TaxCost,
                SubTotal = request.SubTotal,
                TotalPrice = request.TotalPrice,
                Items = request.Items.Select(x => new OrderItemModel
                {
                    Name = x.Name,
                    ExternalProductId = x.ProductId,
                    Sku = x.Sku,
                    Thumbnail = x.Thumbnail,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice
                }).ToList()

            };

            return model;
        }

        private OrderListQueryModel PrepareOrderListQueryModel(OrderListReqeust reqeust)
        {
            var model = new OrderListQueryModel
            {
                States = reqeust.States,
                OrderNumber = reqeust.OrderNumber,
                SortBy = reqeust.SortBy,
                Skip = reqeust.Skip,
                Length = reqeust.Length,
                Desc = reqeust.Desc,
            };

            if(reqeust.StartSubmissionDate != null)
            {
                model.StartSubmissionDate = reqeust.StartSubmissionDate.ToDateTime();
            }

            if(reqeust.EndSubmissionDate != null)
            {
                model.EndSubmissionDate = reqeust.EndSubmissionDate.ToDateTime();
            }

            return model;
        }

        private OrderListResponse PrepareOrderListResponse(PagedResult<OrderDto> paged)
        {
            var response = new OrderListResponse
            {
                Skip = paged.Skip,
                Length = paged.Lenght,
                TotalCount = paged.TotalCount
            };

            foreach (var item in paged.Items)
            {
                response.Items.Add(PrepareOrderResponse(item));
            }

            return response;
        }

        private OrderResponse PrepareOrderResponse(OrderDto order)
        {
            var response = new OrderResponse
            {
                Id = order.Id.ToString(),
                OrderNumber = order.OrderNumber.ToString(),
                UserId = order.UserId,
                ShipmentId = order.ShipmentId,
                PaymentId = order.PaymentId,
                ShippingCost = order.ShippingCost,
                TaxCost = order.TaxCost,
                SubTotal = order.SubTotal,
                TotalPrice = order.TotalPrice,
                BillingAddress = PrepareAddress(order.BillingAddress),
                ShippingAddress = PrepareAddress(order.ShippingAddress),
                CurrentState = order.CurrentState,
                ShippedDate = order.ShippedDate.ToTimestamp(),
                SubmissionDate = order.SubmissionDate.ToTimestamp(),
                
            };

            if(order.Items != null)
            {
                foreach (var item in order.Items)
                {
                    var orderItem = PrepareOrderItemResponse(item);

                    response.Items.Add(orderItem);
                }
            }

            return response;
        }

        private Address PrepareAddress(AddressDto address)
        {
            return new Address
            {
                Name = address.Name,
                CountryCode = address.CountryCode,
                City = address.City,
                StateProvince = address.State,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                PostalCode = address.PostalCode,
                Phone = address.Phone,
                Zip = address.Zip
            };
        }

        private AddressModel PrepareAddressModel(Address address)
        {
            return new AddressModel
            {
                Name = address.Name,
                CountryCode = address.CountryCode,
                City = address.City,
                State = address.StateProvince,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                PostalCode = address.PostalCode,
                Phone = address.Phone,
                Zip = address.Zip
            };
        }
        private OrderItemResponse PrepareOrderItemResponse(OrderItemDto orderItem)
        {
            return new OrderItemResponse
            {
                Id = orderItem.Id.ToString(),
                Name = orderItem.Name,
                Sku = orderItem.Sku,
                ProductId = orderItem.ExternalProductId,
                Thumbnail = orderItem.Thumbnail,
                Quantity = orderItem.Quantity,
                UnitPrice = orderItem.UnitPrice
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
