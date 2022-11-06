using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.ShoppingCart.Application.Abstraction.Dtos;

namespace MicroStore.ShoppingCart.Application.Abstraction.Commands
{
    public class AddBasketItemCommand : ICommand<BasketDto>
    {
        public Guid BasketId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
