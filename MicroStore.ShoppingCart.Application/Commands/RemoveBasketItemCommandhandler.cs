using MicroStore.BuildingBlocks.InMemoryBus;
using MicroStore.ShoppingCart.Application.Abstraction.Commands;
using MicroStore.ShoppingCart.Application.Abstraction.Dtos;
using MicroStore.ShoppingCart.Domain.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace MicroStore.ShoppingCart.Application.Commands
{
    public class RemoveBasketItemCommandhandler : CommandHandler<RemoveBasketItemCommand, BasketDto>
    {
        private readonly IRepository<Basket> _basketRepository;

        private readonly ICurrentUser _currentUser;

        public RemoveBasketItemCommandhandler(IRepository<Basket> basketRepository, ICurrentUser currentUser)
        {
            _basketRepository = basketRepository;
            _currentUser = currentUser;
        }

        public override async Task<BasketDto> Handle(RemoveBasketItemCommand request, CancellationToken cancellationToken)
        {
            Basket? basket = await _basketRepository.SingleOrDefaultAsync(x => x.Id == request.BasketId);

            if (basket == null)
            {
                throw new EntityNotFoundException(typeof(Basket));
            }

            if (!basket.IsBasketItemExist(request.BasketItemId))
            {
                throw new EntityNotFoundException(typeof(BasketItem), request.BasketItemId);
            }

            basket.RemoveItem(request.BasketItemId);

            await _basketRepository.UpdateAsync(basket);

            return ObjectMapper.Map<Basket, BasketDto>(basket);
        }
    }
}
