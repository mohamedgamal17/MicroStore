using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.ShoppingCart.Application.Abstraction.Dtos;
namespace MicroStore.ShoppingCart.Application.Abstraction.Commands
{
    public class RemoveBasketItemCommand : ICommand<BasketDto>
    {
        public Guid BasketId { get; set; }
        public Guid BasketItemId { get; set; }
    }
}
