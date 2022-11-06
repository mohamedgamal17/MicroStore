using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.ShoppingCart.Application.Abstraction.Dtos;

namespace MicroStore.ShoppingCart.Application.Abstraction.Commands
{
    internal class CheckoutCommand : ICommand<OrderDto>
    {
        public Guid BasketId { get; set; }
        public Guid BillingAddressId { get; set; }
        public Guid ShippingAddressId { get; set; }

    }
}
