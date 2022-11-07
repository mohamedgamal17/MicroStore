using MassTransit;
using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.ShoppingCart.Application.Abstraction.Dtos;
using MicroStore.ShoppingCart.Domain.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using Volo.Abp;
using MicroStore.ShoppingCart.Application.Abstraction.Commands;
using MicroStore.Ordering.IntegrationEvents;
using MicroStore.Ordering.IntegrationEvents.Models;
using MicroStore.Ordering.IntegrationEvents.Responses;

namespace MicroStore.ShoppingCart.Application.Commands
{
    public class ChekoutCommandHandler : CommandHandler<CheckoutCommand, OrderDto>, ITransientDependency
    {



        private readonly ICurrentUser _currentUser;
        private readonly IRepository<Basket> _basketRepository;
        private readonly IRequestClient<SubmitOrderIntegrationEvent> _orderRequestClinet;


        public ChekoutCommandHandler(ICurrentUser currentUser, IRepository<Basket> basketRepository, IRequestClient<SubmitOrderIntegrationEvent> orderRequestClinet)
        {
            _currentUser = currentUser;
            _basketRepository = basketRepository;
            _orderRequestClinet = orderRequestClinet;
        }

        public override async Task<OrderDto> Handle(CheckoutCommand request, CancellationToken cancellationToken)
        {
            Basket? basket = await _basketRepository.SingleOrDefaultAsync(x => x.Id == request.BasketId);

            if (basket == null)
            {
                throw new EntityNotFoundException(typeof(Basket), request.BasketId);
            }

            if (basket.LineItems.Count() == 0)
            {
                throw new UserFriendlyException("basket must contain items to preform checkout action");
            }


            SubmitOrderIntegrationEvent integrationEvent = new SubmitOrderIntegrationEvent
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = Guid.NewGuid().ToString(),
                SubTotal = basket.CalculateTotalPrice(),
                TotalPrice = basket.CalculateTotalPrice(),
                OrderItems = MapShoppingCartLineItems(basket.LineItems),
                SubmissionDate = DateTime.UtcNow,
                UserId = _currentUser.Id!.Value.ToString(),
                ShippingAddressId = request.ShippingAddressId,
                BillingAddressId = request.BillingAddressId
            };

            basket.Clear();

            await _basketRepository.UpdateAsync(basket);

            var response = await _orderRequestClinet.GetResponse<OrderSubmitedResponse>(integrationEvent);

            return MapOrderResponse(response.Message);

        }


        private List<OrderItemModel> MapShoppingCartLineItems(IEnumerable<BasketItem> lineItems)
        {
            return lineItems.Select(item => new OrderItemModel
            {
                ProductId = item.Product.Id,
                ItemName = item.Product.Name,
                UnitPrice = item.Product.Price,
            }).ToList();
        }


        private OrderDto MapOrderResponse(OrderSubmitedResponse response)
        {
            return new OrderDto()
            {
                OrderId = response.OrderId,
                UserId = response.UserId,
                BillingAddressId = response.BillingAddressId,
                ShippingAddressId = response.ShippingAddressId,
                SubTotal = response.SubTotal,
                Total = response.Total,
                OrderItems = response.OrderItemModels.Select(x => new OrderItemDto
                {
                    ItemName = x.ItemName,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice
                }).ToList()
            };
        }

    }
}
